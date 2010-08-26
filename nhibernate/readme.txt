When a database is 'opened' call:

NHBootStrapper.Initialize("C:\\path\\to\\mydatabase.sdf");

Then in your code you can use:

var session = NHBootStrapper.SessionFactory.CreateSession();

to create a new NH session