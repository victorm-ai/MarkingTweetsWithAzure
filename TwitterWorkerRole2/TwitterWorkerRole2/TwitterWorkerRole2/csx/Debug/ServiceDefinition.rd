<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="TwitterWorkerRole2" generation="1" functional="0" release="0" Id="f3a88edd-97f4-467f-94fc-e881e5593916" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="TwitterWorkerRole2Group" generation="1" functional="0" release="0">
      <settings>
        <aCS name="MyTwitterWorlerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/TwitterWorkerRole2/TwitterWorkerRole2Group/MapMyTwitterWorlerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="MyTwitterWorlerRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/TwitterWorkerRole2/TwitterWorkerRole2Group/MapMyTwitterWorlerRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <maps>
        <map name="MapMyTwitterWorlerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/TwitterWorkerRole2/TwitterWorkerRole2Group/MyTwitterWorlerRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapMyTwitterWorlerRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/TwitterWorkerRole2/TwitterWorkerRole2Group/MyTwitterWorlerRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="MyTwitterWorlerRole" generation="1" functional="0" release="0" software="D:\Dropbox\My Hard Disk\MVP\Contribuciones\Material\Compute\TwitterWorkerRole2\TwitterWorkerRole2\TwitterWorkerRole2\csx\Debug\roles\MyTwitterWorlerRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;MyTwitterWorlerRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;MyTwitterWorlerRole&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/TwitterWorkerRole2/TwitterWorkerRole2Group/MyTwitterWorlerRoleInstances" />
            <sCSPolicyUpdateDomainMoniker name="/TwitterWorkerRole2/TwitterWorkerRole2Group/MyTwitterWorlerRoleUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/TwitterWorkerRole2/TwitterWorkerRole2Group/MyTwitterWorlerRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="MyTwitterWorlerRoleUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="MyTwitterWorlerRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="MyTwitterWorlerRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
</serviceModel>