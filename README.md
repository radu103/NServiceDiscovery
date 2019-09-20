# NServiceDiscovery - #.NET Core 2.* service discovery server

Features :

* [READY] .NET Core 2.2 
* [READY] Compatible with Eureka Clients (v1 message format) : Java Spring & .NET NuGets : Steeltoe, Pivotal
* [READY] Multitenant capable (tenant send as header `Authorization :  Bearer {tenantId}-{landscape}`. By default tenant name and type used is : `public-dev` when Authorization header is missing
* [READY] Each instance in-memory store for 
	* apps
	* instances
	* key value store for general & app configuration
	* app dependency requirements (appNames)
* [READY] Deployable in Cloud Foundry (SAP Cloud Platform)
* [TO DO] Sync multiple instances via MQTT broker messages
* [TO DO] Persistence with SAP HANA/Mongo/Redis

## Not in scope yet

AMI metadata processing support for AWS

# Endpoints exposed

[Import Postman Collection file](NServiceDiscovery.postman_collection.json) from repository

# Local test & use

1. git clone & build
2. Open 2 instances of Visual Studio Community 2019
3. Open the projects
4. Start the `NServiceDiscoveryAPI` project in one instance
5. Start the `TestAPI1` project in the second instance
6. Check configurations for `TestAPI1` in the file `appSettings.json` if the ports are not `62771` for API and `54880` for TestAPI1 and replace them

# Deploy to Cloud Foundry

1. git clone & build
2. dotnetcore publish to folder 
3. Edit the manifest.yml in the publish folder
4. Open Command Prompt and `cd` to publish folder
5. `cf api https://api.cf.eu10.hana.ondemand.com` + `cf login` + org / space selection
6. cf push <name_of_app>

# Data structures

## Aplications

```json
{
    "application": [
        {
            "instance": [
                {
                    "appId": "APPID_1",
                    "instanceId": "APPHOST11",
                    "sid": null,
                    "status": "STARTING",
                    "overriddenstatus": "UNKNOWN",
                    "statusPageUrl": "http://APPHOST11:8080/status",
                    "hostName": "APPHOST11",
                    "vipAddress": "APPHOST11",
                    "secureVipAddress": "APPHOST11",
                    "port": {
                        "@enabled": true,
                        "$": 8080
                    },
                    "securePort": {
                        "@enabled": true,
                        "$": 8443
                    },
                    "countryId": "1",
                    "homePageUrl": "http://APPHOST11:8080",
                    "healthCheckUrl": "http://APPHOST11:8080/healthcheck",
                    "secureHealthCheckUrl": "",
                    "isCoordinatingDiscoveryServer": false,
                    "lastUpdatedTimestamp": 637044870057618082,
                    "lastDirtyTimestamp": 637044870057618082,
                    "actionType": "ADDED",
                    "metadata": {
                        "@class": "java.util.Collections$EmptyMap"
                    },
                    "leaseInfo": {
                        "renewalIntervalInSecs": 30,
                        "durationInSecs": 30,
                        "registrationTimestamp": 637044870057680720,
                        "lastRenewalTimestamp": 637044870057680720,
                        "evictionTimestamp": 637044868762713424,
                        "serviceUpTimestamp": 0
                    },
                    "dataCenterInfo": {
                        "@class": "com.netflix.appinfo.InstanceInfo$DefaultDataCenterInfo",
                        "name": "MyOwn"
                    }
                }
            ],
            "configuration": [],
            "requiresApps": [],
            "name": "APPID_1",
            "appGroupName": "",
            "protocol": 0
        }
    ],
    "versions__deltam": "2",
    "apps__hashcode": ""
}
```

## Key Value Store

```json
[
    {
        "key": "testkey",
        "value": "testvalue"
    }
]
```

# MQTT messages for sync

All instances subscribe to the topic and process messages

Topic template name : `NServiceDiscovery-{tenantId}-{landscape}`

### Update & version increased = published by some instance that processed a change (published for all instances listening

```json
{
    "from_instance_id" : "id1",
    "to_instance_id" : "ALL",
	"type" : "UPDATE",
	"message" : {
            "new_version" : 123,
            "new_version_timestamp" : 1234567890
	}
}
```

### Request Last Version = to be published by new instance that has started

```json
{
    "from_instance_id" : "id1",
    "to_instance_id" : "ALL",
    "type" : "START_REQUEST"
}
```

### Start Request Acknowledged = sent back after persistency is synched by only 1 processor that is listening

```json
{
    "from_instance_id" : "id2",
    "to_instance_id" : "id1",
	"type" : "START_REQUEST",
	"message" : {
            "new_version" : 123,
            "new_version_timestamp" : 1234567890,
            "persistency_connection_string" : "here"
	}
}
```

# Cool tools used

* Free Public Online MQTT broker : [HiveMQ](http://www.mqtt-dashboard.com/)
* For MQTT publish and monitor : [mqqt-spy](https://github.com/eclipse/paho.mqtt-spy/wiki/Downloads)
* Eureka Client for .NET Framework : [Steeltoe Discovery](https://steeltoe.io/docs/steeltoe-discovery/) 
