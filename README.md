
# Unity3DSocketAsync

This is a project of Socket Async in Unity3D. The server is NodeJS.

First you need to download and install the [**NodeJS**]([https://nodejs.org/en/](https://nodejs.org/en/)), after this, open the folder **NodeSocket** and run the command:

```
npm install
```

After:

```
npm install nodemon -g
```

And finally:

```
nodemon
```

The server will start on port **3009**.

Open the Unity3D, and after the file **SocketConfig** and change the configuration:

```csharp
 public class SocketConfig
 {
     public const string IP = "192.168.1.107"; //your current IP
     public const int Port = 3009; //port of server
 }
```

Now, put the path of file, load and send to the server.
