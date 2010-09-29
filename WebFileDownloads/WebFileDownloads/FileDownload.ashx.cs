using System;
using System.IO;
using System.Linq;
using System.Web;

namespace WebFileDownloads
{
    public class FileDownload : IHttpHandler
    {
        #region IHttpHandler Members

        public void ProcessRequest(HttpContext context)
        {
            //string filePath = context.Server.MapPath("~/DotNetBookZero11.pdf");
            string filePath = context.Server.MapPath("~/Default.aspx");

            if (!File.Exists(filePath))
            {
                //  todo: perhaps log a warning or error here

                //  return a 404 error to the browser saying that
                //  we couldn't find the file requested
                context.Response.StatusCode = 404;
                context.Response.Status = "404 Not found";
            }
            else
            {
                //  don't buffer the output in memory before sending it to the browser
                //  send it directly to the browser as soon as possible
                context.Response.BufferOutput = false;

                using (var fs = new FileStream(filePath, FileMode.Open,
                                               //  only open the file for reading
                                               FileAccess.Read,
                                               //  allow other programs to open the file for reading
                                               //  at the same time, but don't let them change or delete
                                               //  the file until we are done with it
                                               FileShare.Read))
                {
                    //  Files could change at anytime, we always want to send the
                    //  most recent copy of the file, so don't allow the browser
                    //  or the server to cache our response.
                    context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

                    //  This is where we tell the browser exactly what 'type'
                    //  of file we are sending it so that the browser can
                    //  make an informed decision about how to handle it.
                    context.Response.ContentType = DetermineContentType(filePath);

                    //  check the querystring to see whether we have been asked
                    //  to force a download or show the file inline
                    bool forceDownload = context.Request.Url.Query != "?inline";
                    AddContentDispositionHeader(filePath, forceDownload, context.Response);

                    //  Specifying the content-length helps avoid some issues
                    //  with various browser quirks by letting the browser know
                    //  in advance exactly how big the file is that we are sending it.
                    context.Response.AddHeader("content-length", fs.Length.ToString());

                    WriteFileToResponse(fs, context.Response);
                }
            }
        }

        public bool IsReusable
        {
            get { return true; }
        }

        #endregion

        private static string DetermineContentType(string filePath)
        {
            string ext = Path.GetExtension(filePath);
            if (ext != null)
            {
                ext = ext.ToLowerInvariant();
            }

            switch (ext)
            {
                case ".pdf":
                    return "application/pdf";
                case ".aspx":
                    return "text/plain";
                    //  Use "application/octet-stream" as a default
                    //  if you don't know a more specific file type.
                default:
                    return "application/octet-stream";
            }
        }

        private static void AddContentDispositionHeader(string filePath, bool forceDownload, HttpResponse response)
        {
            //  this is the default file name that the browser
            //  will offer to save the file as to the user's computer
            string defaultDownloadFileName = Path.GetFileName(filePath);

            if (forceDownload)
            {
                //  Adding this header will force the browser to download
                //  the file rather than show it 'inline' in the browser window.
                //  In IE this will show the Open/Save As prompt.
                response.AddHeader("content-disposition",
                                   string.Format("attachment; filename=\"{0}\"",
                                                 defaultDownloadFileName));
            }
            else
            {
                //  Adding this header will tell the browser to show the file
                //  'inline' in the browser window.
                //  However we still give the browser a default filename incase
                //  showing this type of file inline isn't supported and the user
                //  is required to download it first.
                response.AddHeader("content-disposition",
                                   string.Format("inline; filename=\"{0}\"",
                                                 defaultDownloadFileName));
            }
        }

        private static void WriteFileToResponse(FileStream fs, HttpResponse response)
        {
            var buffer = new byte[1024]; // 1 KB buffer

            //  keep track of how much of the file we have read
            //  and stop when we have read the entire file
            long numBytesRead = 0;
            while (numBytesRead < fs.Length)
            {
                int numBytesReadIntoBuffer = fs.Read(buffer, 0, buffer.Length);

                if (numBytesReadIntoBuffer == buffer.Length)
                {
                    //  if we filled the buffer, then write the entire buffer
                    //  to the response
                    response.BinaryWrite(buffer);
                }
                else
                {
                    //  if we only filled part of the buffer, then get
                    //  the part we filled and write just that portion
                    //  to the response
                    byte[] bytesToWrite = buffer.Take(numBytesReadIntoBuffer).ToArray();
                    response.BinaryWrite(bytesToWrite);
                }

                numBytesRead += numBytesReadIntoBuffer;
            }
        }
    }
}