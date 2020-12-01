# An example project for using Serilog

Like many other libraries for .NET, Serilog provides diagnostic logging to files, the console, and elsewhere. It is easy to set up, has a clean API, and is portable between recent .NET platforms.

Unlike other logging libraries, Serilog is built with powerful structured event data in mind.

For more details https://serilog.net/

## Run project
I configured logs to be sinked to Seq.  Seq is the intelligent search, analysis, and alerting server built specifically for modern structured log data.
For details https://datalust.co/. (If you want, you can disable by setting empty value to SeqUrl in appsettings.json.)

Go to src folder. Execute the following commands.

 src > docker run \
  --name seq \
  -d \
  --restart unless-stopped \
  -e ACCEPT_EULA=Y \
  -v /path/to/seq/data:/data \
  -p 80:80 \
  -p 5341:5341 \
  datalust/seq:latest
  
 src > dotnet run
 
 
