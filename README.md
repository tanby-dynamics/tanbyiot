# tanbyiot

IoT intelligence platform

## Developing

1. Run `docker compose up -d`
2. Go to `src/Migrations` and `dotnet run`
3. Go to `src/tanbyiot.client` and `npm install`
4. Open the solution and run the `tanbyiot.Server` project. The `tanbyiot.client` application should load as well.
5. Use the `mockdevice` tool to test the device-facing APIs
