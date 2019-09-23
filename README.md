# NServiceDiscovery - Cloud Service Discovery & Configuration Server (.NET Core 2.*)

## Features

* [READY] .NET Core 2.2 
* [READY] Nearly Real-Time Sync of peers informations' using MQTT Broker broadcast messages (and eviction worker with configurable period of validity)
* [READY] Much smaller memory footprint tha Eureka
    * empty NServiceDiscovery instance size = 40 MB in Cloud Foundry, around 100MB in IIS (15 times less memory consumed)
    * empty Eureka Server instance size     = 620 MB (a lot for any application that is just started)
* [READY] Compatible with Eureka Clients (v1 message format) : Java Spring & .NET NuGets : Steeltoe, Pivotal
* [READY] Multitenant capable (tenant send as header `Authorization :  Bearer {tenantId}-{landscape}`. By default tenant name and type used is : `public-dev` when Authorization header is missing
* [READY] Each instance in-memory store for 
	* apps
	* instances
	* key value store for general & app configuration
	* app dependency requirements (appNames)
* [READY] Deployable in Cloud Foundry (SAP Cloud Platform)
* [TO DO] Persistence with SAP HANA / Mongo/Redis
* [TO DO] Sync multiple instances via MQTT broker messages (save to persistency by the changer & load from persistency on mqqt message by the others)

## Not in scope yet

AMI metadata processing support for AWS, Azure, Pivotal CF

## Endpoints exposed

[Import Postman Collection file](./NServiceDiscoveryAPI/NServiceDiscovery.postman_collection.json) from repository

# Environment variables needed 

### Mandatory

ASPNETCORE_URLS = the public urls (needed for peer broadcast)

### Optional and defaults

[defaults values here](./NServiceDiscovery/Configuration/DefaultConfigurationData.cs)

# HOW-TO-GUIDES

## Local test & use

1. git clone & build
2. Open 2 instances of Visual Studio Community 2019
3. Open the projects
4. Start the `NServiceDiscoveryAPI` project in one instance
5. Start the `TestAPI1` project in the second instance
6. Check configurations for `TestAPI1` in the file `appSettings.json` if the ports are not `62771` for API and `54880` for TestAPI1 and replace them

## Deploy to Cloud Foundry

1. git clone & build
2. dotnetcore publish to folder 
3. Edit the manifest.yml in the publish folder
4. Open Command Prompt and `cd` to publish folder
5. `cf api https://api.cf.eu10.hana.ondemand.com` + `cf login` + org / space selection
6. cf push <name_of_app>

## Create your own NServiceDiscovery Docker Container Image

1. git clone & build (in below example I cloned to : `C:\Work\NServiceDiscovery`)
2. Open `cmd` or powershell
3. Build : `docker build -f "C:\Work\NServiceDiscovery\NServiceDiscoveryAPI\Dockerfile" -t nservicediscovery  "C:\Work\NServiceDiscovery"`
4. Run local : 
   * instance 1 : `docker run -d -p 18771:8771 --name NServiceDiscovery1 nservicediscovery`
   * instance 2 : `docker run -d -p 28771:8771 --name NServiceDiscovery2 nservicediscovery`
5. Check : `docker ps -a` and `docker logs -ft <container_id>`
6. See container IP address : `docker inspect -f "{{ .NetworkSettings.Networks.bridge.IPAddress }}" NServiceDiscovery1`
7. Login Publish to Docker Hub with  `docker login` and 
   * `docker tag <image_id> radu103/nservicediscovery:latest`
   * `docker push radu103/nservicediscovery:latest`

# Data structures

## Aplications structure

```json
{
    "applications": {
        "versions__delta": 1,
        "apps__hashcode": "UP_0_",
        "application": [
            {
                "instance": [
                    {
                        "app": "APPID_1",
                        "appGroupName": null,
                        "instanceId": null,
                        "status": "STARTING",
                        "overriddenStatus": "UNKNOWN",
                        "hostName": "APPHOST11",
                        "ipAddr": "10.0.0.10",
                        "sid": null,
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
                        "countryId": 1,
                        "homePageUrl": "http://APPHOST11:8080",
                        "healthCheckUrl": "http://APPHOST11:8080/healthcheck",
                        "statusPageUrl": "http://APPHOST11:8080/status",
                        "secureHealthCheckUrl": "",
                        "isCoordinatingDiscoveryServer": false,
                        "lastUpdatedTimestamp": 1569044028051,
                        "lastDirtyTimestamp": 1569044028051,
                        "actionType": "ADDED",
                        "metadata": null,
                        "leaseInfo": {
                            "renewalIntervalInSecs": 30,
                            "durationInSecs": 30,
                            "lastHealthCheckDurationInMs": 0,
                            "registrationTimestamp": 1569044028051,
                            "lastRenewalTimestamp": 1569044028051,
                            "evictionTimestamp": 1567749060755,
                            "serviceUpTimestamp": 1569044028051
                        },
                        "dataCenterInfo": {
                            "@class": "com.netflix.appinfo.InstanceInfo$DefaultDataCenterInfo",
                            "name": "MyOwn"
                        }
                    }
                ],
                "configuration": [
                    {
                        "key": "confkey1",
                        "value": "value1"
                    },
                    {
                        "key": "confkey2",
                        "value": "value2"
                    },
                    {
                        "key": "confkey3",
                        "value": "value3"
                    }
                ],
                "dependencies": [
                    "APPID_2",
                    "APPID_3"
                ],
                "name": "APPID_1"
            }
        ]
    }
}
}
```

## General Key Value Store structure

```json
[
    {
        "key": "testkey1",
        "value": "testvalue1"
    },
    {
        "key": "testkey2",
        "value": "testvalue2"
    },    
    {
        "key": "testkey3",
        "value": "testvalue3"
    }    
]
```

# Persistency structure

Based only on 2 tables with INSERT & SELECT operations to have for perfect history tracing of landscape evolution at all moments in the past.

## Table for Applications

1. {TenantId}-{TenantType} = String
2. Version = Long
3. Timestamp = Timestamp
4. JSON String with AllAplications object serialized

## Table for General KeyValue Store

1. {TenantId}-{TenantType} = String
2. Version = Long
3. Timestamp = Timestamp
4. JSON String with the General Key Value Store object serialized

# MQTT messages for sync

All instances subscribe to the topic and process messages

Topic template name : `NServiceDiscovery-{tenantId}-{landscape}`

### INSTANCE_CONNECTED = to be published by new instance that has started

```json
{
    "from_instance_id" : "id1",
    "to_instance_id" : "ALL",
    "type" : "INSTANCE_CONNECTED",
    "message" : {
         "peerId" : "ServerInstanceID",
         "discoveryUrls" : "url",
    }
}
```


### INSTANCE_HEARTBEAT = to be published by new instance that has started

```json
{
    "from_instance_id" : "id1",
    "to_instance_id" : "ALL",
    "type" : "INSTANCE_HEARTBEAT",
    "message" : {
         "peerId" : "ServerInstanceID",
         "discoveryUrls" : "url",
    }
}
```

# Cool tools used

* Free Public Online MQTT broker : [HiveMQ](http://www.mqtt-dashboard.com/)
* For MQTT publish and monitor : [mqqt-spy](https://github.com/eclipse/paho.mqtt-spy/wiki/Downloads)
* MQTT Client & Server for .NET frameworks : [MQTTnet](https://github.com/chkr1011/MQTTnet)
* Eureka Client for .NET Framework : [Steeltoe Discovery](https://steeltoe.io/docs/steeltoe-discovery/)
* Visual Studio Cmmunity 2019 : [VS 2019](https://visualstudio.microsoft.com/downloads/)
