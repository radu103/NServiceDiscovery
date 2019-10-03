# NServiceDiscovery - Cloud Service Discovery & Configuration Server (.NET Core 2.*)

## Features

* [READY] .NET Core 2.2 
* [READY] Nearly Real-Time Sync of peers informations' using MQTT Broker broadcast messages (and eviction worker with configurable period of validity)
* [READY] Much smaller memory footprint tha Eureka
    * empty NServiceDiscovery instance size = 28 MB in Cloud Foundry, around 100 MB in IIS Express on Debug mode (more than 100% less memory consumed)
    * empty Eureka Server instance size     = 66 MB
* [READY] Compatible with Eureka Clients (v1 message format) : Java Spring & .NET NuGets : Steeltoe, Pivotal
* [READY] Multitenant capable (tenant send as header `Authorization :  Bearer {tenantId}-{landscape}`. 
          By default tenant name and type used is : `public-dev` when Authorization header is missing
* [READY] Each instance in-memory store for 
	* apps
	* instances
	* key value store for general & app configuration
	* app dependency requirements (appNames)
* [READY] Deployable in Cloud Foundry (SAP Cloud Platform)
* [READY] Expired metadata removal
    * [READY] Automatic instance eviction task if no heartbeat http message received
    * [READY] Automatic peer removal task if no heartbeat mqtt message received
* [WORK IN PROGRESS] Sync metadata in memory for multiple instances via MQTT broadcast messages for known peers with QoS 1 (at least once)
    * [READY] Sync of instances (add, update & delete) using mqtt messages
    * [READY] Sync of discovery & configuration clients hostnames that have activity
    * [TO DO] Sync of app dependencies & app configurations using mqtt messages
    * [TO DO] Sync of general configuration using mqtt messages
* [WORK IN PROGRESS] UI5 Overview Cockpit web user interface
* [TO DO] Auto select persistency type on Cloud Foundry vased on VCAP_SERVICES binded to app
* [TO DO] Auto select persistency type based on CF environment variables
* [TO DO] Persistence save & load with SAP HANA / Mongo / Redis

## Not in scope yet

AMI metadata processing support for AWS, Azure, Pivotal CF

## Endpoints exposed

[Import Postman Collection file](./NServiceDiscoveryAPI/NServiceDiscovery.postman_collection.json) from repository

# Environment variables needed 

[defaults values here](./NServiceDiscovery/Configuration/DefaultConfigurationData.cs)

### Mandatory configuration

ASPNETCORE_URLS = the public urls (needed for peer broadcast)

### Optional configuration

On CF : SINGLE_TENANT_ID & SINGLE_TENANT_TYPE user providedd ENV VARIABLES if you want to block the functionality to a single tenant (named by configuration)

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
         "discoveryUrls" : "url"
    }
}
```

### INSTANCE_HEARTBEAT = to be published by all instances at constant interval

```json
{
    "from_instance_id" : "id1",
    "to_instance_id" : "ALL",
    "type" : "INSTANCE_HEARTBEAT",
    "message" : {
         "peerId" : "ServerInstanceID",
         "discoveryUrls" : "url"
    }
}
```

### CLIENT_DISCOVERY_ACTIVITY = to be published by all instances when a client gets application(s) or delta info

```json
{
    "from_instance_id" : "id1",
    "to_instance_id" : "ALL",
    "type" : "CLIENT_DISCOVERY_ACTIVITY",
    "message" : {
        "tenantId" : "string",
        "clientHostname" : "hostname",
        "lastUpdateTimestamp" : "utc datetime"
    }
}
```

### CLIENT_CONFIGURATION_ACTIVITY = to be published by all instances when a client gets general configuration

```json
{
    "from_instance_id" : "id1",
    "to_instance_id" : "ALL",
    "type" : "CLIENT_GENERAL_CONFIGURATION_ACTIVITY",
    "message" : {
        "tenantId" : "string",
        "clientHostname" : "hostname",
        "lastUpdateTimestamp" : "utc datetime"
    }
}
```

### PERSISTENCY_SYNC_APPS = to be published by one instance at constant interval. Value of interval with be randomized between 5 and 10 minutes on instance startup

```json
{
    "from_instance_id" : "id1",
    "to_instance_id" : "ALL",
    "type" : "PERSISTENCY_SYNC",
    "message" : {
         "peerId" : "id1",
         "tenantId" : "id-type",
         "appsMd5Hash" : "string"
    }
}
```

### PERSISTENCY_SYNC_APPS_RESPONSE = response sent back after an PERSISTENCY_SYNC by all receiving peers

```json
{
    "from_instance_id" : "id1",
    "to_instance_id" : "id2",
    "type" : "PERSISTENCY_RESPONSE",
    "message" : {
         "peerId" : "id1",
         "tenantId" : "id-type",
         "appsMd5Hash" : "string"
    }
}
```

### PERSISTENCY_SYNC_KEYS = to be published by one instance at constant interval. Value of interval with be randomized between 5 and 10 minutes on instance startup

```json
{
    "from_instance_id" : "id1",
    "to_instance_id" : "ALL",
    "type" : "PERSISTENCY_SYNC",
    "message" : {
         "peerId" : "id1",
         "tenantId" : "id-type",
         "keysMd5Hash" : "string"
    }
}
```

### PERSISTENCY_SYNC_KEYS_RESPONSE = response sent back after an PERSISTENCY_SYNC by all receiving peers

```json
{
    "from_instance_id" : "id1",
    "to_instance_id" : "id2",
    "type" : "PERSISTENCY_RESPONSE",
    "message" : {
         "peerId" : "id1",
         "tenantId" : "id-type",
         "keysMd5Hash" : "string"
    }
}
```

### ADD_INSTANCE and UPDATE_INSTANCE = published when app instance is added or updated

```json
{
    "from_instance_id": "ea42ea39-8959-4e01-a72e-44467239f1c5",
    "to_instances_ids": [
            "c109e4e7-101f-4923-729d-ebc7",
            "48e7d808-f425-4858-4824-f3fd",
            "01341a0e-8ce8-4e80-4c7a-9d4a",
            "3683bf4a-869a-4b22-47cb-6ce5",
            "9b00842e-df61-4120-46f1-6099",
            "928018ca-ea58-49cb-6a84-c49e",
            "2b4a3880-f1f3-4c6a-7c15-b6d0",
            "bb06fa92-2013-493a-51a5-4a0f"
    ],
    "type": "ADD_UPDATE_INSTANCE",
    "message": "{'app':'APPID_1','appGroupName':null,'instanceId':'APPHOST11','status':'STARTING','overriddenStatus':'UNKNOWN','hostName':'APPHOST11','ipAddr':'10.0.0.10','sid':null,'vipAddress':'APPHOST11','secureVipAddress':'APPHOST11','port':{'@enabled':true,'$':8080},'securePort':{'@enabled':true,'$':8443},'countryId':1,'homePageUrl':'http://APPHOST11:8080','healthCheckUrl':'http://APPHOST11:8080/healthcheck','statusPageUrl':'http://APPHOST11:8080/status','secureHealthCheckUrl':'','isCoordinatingDiscoveryServer':false,'lastUpdatedTimestamp':1569608540834,'lastDirtyTimestamp':1569608540834,'actionType':'ADDED','metadata':null,'leaseInfo':{'renewalIntervalInSecs':30,'durationInSecs':90,'lastHealthCheckDurationInMs':0,'registrationTimestamp':1569608540834,'lastRenewalTimestamp':1569608540834,'evictionTimestamp':1569908540834,'serviceUpTimestamp':1569608540835},'dataCenterInfo':{'@class':'com.netflix.appinfo.InstanceInfo$DefaultDataCenterInfo','name':'MyOwn'}}"
}
```

### CHANGE_INSTANCE_STATUS = published when app instance status is updated

```json
{
    "from_instance_id" : "id1",
    "to_instance_ids" : ["id2"],
    "type" : "DELETE_INSTANCE",
    "message" : {
        "appName" : "string", 
        "tenantId" : "string", 
        "instanceId" : "string",
        "status" : "string",
        "lastDirtyTimestamp" : 1242
    }
}
```

### DELETE_INSTANCE = published when app instance is added or updated

```json
{
    "from_instance_id" : "id1",
    "to_instance_ids" : ["id2"],
    "type" : "DELETE_INSTANCE",
    "message" : {
        "appName" : "string", 
        "tenantId" : "string", 
        "instanceId" : "string"
    }
}
```

### ADD_APP_DEPENDENCIES = published when app dependencies are deleted

```json
{
    "from_instance_id" : "id1",
    "to_instance_ids" : ["id2"],
    "type" : "DELETE_APP_DEPENDENCIES",
    "message" : {
        "appName" : "string", 
        "tenantId" : "string", 
        "addedDependencies" : ["string"]
    }
}
```

### DELETE_APP_DEPENDENCIES = published when app dependencies are deleted

```json
{
    "from_instance_id" : "id1",
    "to_instance_ids" : ["id2"],
    "type" : "DELETE_APP_DEPENDENCIES",
    "message" : {
        "appName" : "string", 
        "tenantId" : "string", 
        "deletedDependencies" : ["string"]
    }
}
```

# Persistency consensus algorithm

* Runned for each tenant individually
* When the instance launches the `PERSISTENCY_SYNC` the Persistency objects are backed up
* If the instance receives at least N / 2 (half of no peer nodes registered already for the tenant) responses with same hashes than the instance saves its data to persistency
* All instances receiving `PERSISTENCY_SYNC` message with backup the state and save the content for validation
* All instances that see N / 2 messsages with identical values for hash after `PERSISTENCY_SYNC` message in the next minute will save to persistency

`MD5Hash` property algorithm :

    * start with empty string var
    * filter apps by TenantId & Type 
    * order apps by name
    * append `<AppName>_`
    * for each app, filter instances by status = UP:
        * order instances that have status = UP by id
        * append `<instance_id>_`
    * compute MD5 string hash on the string var in the end
 
```json
{
    "from_instance_id" : "id1",
    "to_instance_id" : "ALL",
    "type" : "INSTANCE_CONNECTED",
    "message" : {
        "tenantId" : "public",
        "tenantType" : "dev",
        "md5Hash" : "md5string"
    }
}
```

# Cool tools used

* Free Public Online MQTT broker : [HiveMQ](http://www.mqtt-dashboard.com/)
* For MQTT publish and monitor : [mqqt-spy](https://github.com/eclipse/paho.mqtt-spy/wiki/Downloads)
* MQTT Client & Server for .NET frameworks : [MQTTnet](https://github.com/chkr1011/MQTTnet)
* Eureka Client for .NET Framework : [Steeltoe Discovery](https://steeltoe.io/docs/steeltoe-discovery/)
* Visual Studio Cmmunity 2019 : [VS 2019](https://visualstudio.microsoft.com/downloads/)
