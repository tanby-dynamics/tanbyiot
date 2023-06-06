# Edge IoT
IoT intelligence platform

## Developing

### Front-end
1. Install node 20.0.0
2. Install yarn and next: `npm install -g yarn next`
3. Start a powershelll and enable unsigned scripts (it's cool trust me just don't read about reverse shell attacks...): `Set-ExecutionPolicy -ExecutionPolicy Unrestricted`
4. Install dependencies: `yarn`
5. Start development server: `yarn dev`

## Backlog
- [ ] Web API endpoint that can receive telemetry
- [ ] Mock device testbench
- [ ] Azure Functions app that receives a telemetry event, just marks it as processed
- [ ] Write telemetry to Cosmos DB
- [ ] ESP32 library that can register
- [ ] ESP32 function to send telemetry
- [ ] Instead of continuing to use the device ApiKey, get a token
- [ ] MVP rules engine
- [ ] First rule trigger: if telemetry contains a particular value
- [ ] Rule trigger: webhook: perform a POST or GET to a url containing both the telemetry and custom data
- [ ] WebJob service for scheduled tasks
- [ ] Time-based rule trigger at a particular time
- [ ] Flesh out time-based rules
- [ ] Subscription - many users per subscription, many devices per subscription
- [ ] Implement security in front-end and API
- [ ] Device groups
- [ ] Devices should use a bearer token instead of just passing the token in the request body
- [ ] Register devices properly
- [ ] Action to send an email
- [ ] Action to send instruction to device
- [ ] Device libs should be able to register for instructions
- [ ] Device libs should poll for instructions; array, execute all in order
- [ ] 