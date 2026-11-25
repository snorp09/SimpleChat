### Message
A quick overview of what is a Message. In SimpleChat, a message is a string sent over the socket, in UTF-8 encoding. To keep it simple (hence the name), a command simple breaks into three components.
```
|[Source] [Command] [Arguments]|
```
A break down of the components are as follow:
###### Source:
Source is simple, this is object sending the message. This is conventionally a user, but in theory could be another, such as an application.
###### Command
The command is the name of the command to execute. Conceptually, this should be checked against a permission table to decide if a user is allowed to execute said command, but will be omitted to keep things simple.

###### Arguments
Arguments are used to aid the command in preforming it's function. This is also pretty self explanatory.

### Communication
```
            [ Server ]
            /       \
           /         \
          /           \
     [Client 1]   [Client 2]
```
Simple Chat works around the common concept of one server, to many clients. Upon connection, the client should send an identification command, with the nickname of the client.

The three basic responses from the server are the following:
###### `Server Received [Code]`
A Received response indicates that the server has gotten the command, but the action has not been preformed yet. This can be due to large data to shift through, or whatever causes a delay, but the client needs to know that the server is alive and has gotten the command.
###### `Server Ok [Code]`
An Okay response means that the server has completed the action associated with the command. This should be directly followed up with a more detailed response with more specific details of the result of the operation.

###### `Server Err [Code]`
An Okay response indicates that an error happened while processing the command. The associated code informs the client as to the nature of the error. Refer to each command for their potential error codes.

Upon a command being accepted, and a Received Response being sent to the client of origin, the server will issue out the proper actions to each applicable client. In many, if not most cases, this is just an echo of the command, to each connected client. 
### Commands
The following is a short list of the commands implemented by a SimpleChat Client and Server.
###### `[Source] Received [Code]`
A Received response indicates that the source has gotten the command, but the action has not been preformed yet. This can be due to large data to shift through, or whatever causes a delay, but the client needs to know that the server is alive and has gotten the command.
###### `[Source] Ok [Code]`
An Okay response means that the source has completed the action associated with the command. This should be directly followed up with a more detailed response with more specific details of the result of the operation.

###### `[Source] Err [Code]`
An Okay response indicates that an error happened while processing the command. The associated code informs the client as to the nature of the error. Refer to each command for their potential error codes.
###### `[UserId] Join [Name]`
Immediately upon connection to the server, the client should send the Join command to the server, with the desired username as an argument. Upon initial connection, the client will not have a UserId to send to the server, so it should use a **Source** set to `NewUser`. This will tell the server to instead of looking up the preexisting user, instead issue a new UserId to the user. 

The command then should be echoed across all connected clients, informing all connected clients that a new user has joined the chat. If this is the user's first connection, the client should store the new UserId found in the *Source* parameter for future messages.
###### `[UserId] Ping`
The Ping command does nothing other than inform each party that each other are still connected. The server should issue a `Ping` command to each client, within an interval configured by the server, to ensure the client is still connected.

The expected response from a `Ping` command is simply an `Ok` message directly at the origin, with no additional responses.
###### `[UserId] Disconnect`
This command is sent to the server when a user is leaving the chat room, or when the client fails to respond to a `Ping` command.

The server upon receiving a `Disconnect`, broadcasts this message out to each connected clients, to inform them of the user's departure. 
###### `[UserId] Say [Message]`
The Say command is the fundamental concept of SimpleChat, as this represents the basic concept of a message. Upon a message being received by the server, the command should be echoed across all connected clients.
