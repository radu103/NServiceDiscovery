# NServiceDiscovery - #.NET Core 2.* service discovery server

Features :

* [READY] .NET Core 2.2 
* [READY] Compatible with Eureka Clients (v1 message format) : Java Spring & .NET NuGets : Steeltoe, Pivotal
* [READY] Multitenant capable (tenant send as header `Authorization :  Bearer {tenantId}-{landscape}` by default tenat `public-dev` considered if header is missing
* [READY] Each instance in-memory store for 
	* apps
	* instances
	* key value store for general & app configuration
	* app dependency requirements (appNames)
* [TO DO] Deployable in Cloud Foundry (SAP Cloud Platform)
* [TO DO] Sync multiple instances via MQTT broker messages
* [TO DO] Persistence with SAP HANA/Mongo/Redis

## Not in scope yet :

AMI metadata processing support for AWS

# Endpoints exposed

[Import Postman Collection file](NServiceDiscovery.postman_collection.json) from repository

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
}```

## Key Value Store

```json
[
    {
        "key": "testkey",
        "value": "testvalue"
    }
]
```