# Tanby IoT

An expressive, open-source, commercial-scale, self-hosted IoT device management platform.

Developed by [Tanby Dynamics](https://tanbydynamics.co) and [Rebecca Scott](https://becdetat.com).

[Commercial support available](https://tanbyiot.app/support).


## Developing

1. Run `docker compose up -d`
2. Go to `src/Migrations` and `dotnet run`
3. Go to `src/tanbyiot.client` and `npm install`
4. Open the solution and run the `tanbyiot.Server` project. The `tanbyiot.client` application should load as well.
5. Use the `mockdevice` tool to test the device-facing APIs
