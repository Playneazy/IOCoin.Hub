# IOCoin (IOC) Headless Wallet

---

A strongly typed headless RPC daemon wrapper for I/O Coin.

### Features:

- Seperate RPC wrapper (IOCoin.Headless) from Wallet functionality (IOCoin.Hub).
  
- 'Headlesss.config' manages all initialization parameters.
  
- No need to have existing wallet running, all commands executed from daemon.
  
- Async RPC calls with ''ResultEventHandler' to subscribe against to manage threading.
  
- Set's up a local HttpListener that runs in the background for blockchain updates from daemon and exposes a 'NotificationEventHandler'.
  
- Seperation of concerns;
  
  - IOCoin.Headless = RPC Daemon Wrapper.
    
  - IOCoin.Wallet = Core 34efeWallet implementation.
    
  - IOCoin.Console = Basic UI.
    

- IWallet default implementation with a 'UpdateWalletEventHandler'.
  
- Known transaction fee's are built into each process. Data fee's, a base price per kb is provided.
  

### Future Upgrades:

- Multiple IOC Wallet support.
  
- Calculate dynamic fee's on data transactions.
  

### Primary Console Commands:

Just type the command in at the command prompt after executing the application.

| Command | Desription |
| --- | --- |
| initwallet | Initializes the wallet using 'Headless.Config' settings and waits until there is a valid connection established via 'getinfo' to the daemon. Most work here deals with getting the daemon setup and running. |
| stake | Executes a series of commands to ensure staking only is enabled. This will set encryption if not set, unlock the wallet if not unlocked already, and checks 'getstakinginfo' to ensure staking is enabled. |
| unlock | Does the same as 'stake' but unlocks the wallet for other functionality. |
| exit | Shuts down the daemon and closes the application. |
| **"command"** | Execute any existing daemon command as normal and get the JSON RPC response. |

## Getting Started:

The IOCoin.Console provides basic implementation of functionality. Please walk yourself through that as a guide.

##### How it works:

1. Create instance of Daemon. The Daemon instance reads from 'Headless.Config' to load required settings. This is where multi-wallet support will start when implemented.
  
2. Create an instance of WebServer in the IOCoin.Headless project. This exposes Block and Wallet notification events for subscription.
  
3. Execute Processes. All processes inherit from IProcess in IOCoin.Headless.
  

Headless.Config example:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<startup>
		<supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5" />
	</startup>
	<appSettings> 
		<add key="daemonpath" value="C:\Users\k\Documents\IOC\IOC_Daemon\iocoind.exe" />
		<add key="appdatadir" value="C:\Users\k\Documents\IOC\IOC_Test_Wallet" /> 
		<add key="configfilepath" value="C:\Users\k\Documents\IOC\IOC_Test_Wallet\iocoin.conf" />
		<add key="walletpassphrase" value="passphrase" />
		<add key="notificationaddress" value="http://localhost:8000/" />

		<add key="addnodes" value="amer.supernode.iocoin.io,emea.supernode.iocoin.io,apac.supernode.iocoin.io" />
		<add key="updateintervalMin" value="5" />

	</appSettings>
</configuration>
```

##### Creating a new Daemon Process:

You start with a 'Daemon' instance from IOCoin.Headless and feed in a wallet instance of IWallet type.

```csharp
ConsoleWriter.Info($"Initializing daemon and files...");
Daemon = new Daemon(Wallet);
LoadSettings();
```

The Daemon instance will load settings from Headless.config and make sure some other things are taken care of. As an example we can then call a local LoadSettings(), which loads application settings that piggy back off Headless.Config as shown in IOCoin.Console's Main program.

##### Creating a new Process Response:

Processes have everything they need from the Daemon instance. Here's an example of some processes being called after the Daemon instance has been created.

```csharp
switch (command)
{
    case "registeralias":
        var alias = cmdArgs?.ElementAtOrDefault(1);
        if (string.IsNullOrEmpty(alias))
        {
            var aliasHash = await new RegisterAlias(Daemon.settings, wallet).Run(cmdArgs.ElementAtOrDefault(1));
            await new AliasList(Daemon.settings, wallet).Run();
        }                       
    break;
    case "aliaslist":
        var aliases = await new AliasList(Daemon.settings, wallet).Run();
        break;
    case "listtransactions":
        var transactions = await new ListTransactions(Daemon.settings, wallet).Run();
        break;
    case "stakinginfo":
        var stakeInfo = await new GetStakingInfo(Daemon.settings, wallet).Run();
        break;
    default:
        break;
}
```

Each of these processes have abilities to update the wallet automatically.

Let's take a look at the AliasList process:

```csharp
public class AliasList : ProcessBase<List<AliasResponse>>
```

What's important to note is the inheritance of the ProcessBase class. It brings in some important functionality as well as defines the Result object type of the process.

Next we start by defining the ProcessStartInfo.

```csharp
public async Task<AliasList> Run()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = settings.daemonArgBase + "aliasList";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true
```

We then pass the startInfo into the StartProcess function inside the ProcessBase class.

```csharp
await StartProcess(true, startInfo);
```

The first 'true' arguement shown above awaits the thread so the next calls can begin to explore the underlying RPC results. The StartProcess handles some other background functionality to start the process, manage timeouts, and update results as well.

Next we process the results:

```csharp
var res = JsonConvert.DeserializeObject<List<AliasResponse>>(Rpc.OutputMsg);
Wallet.Aliases = res;
await HandleResult(res, startInfo, true);
```

The above code shows how to convert a serialize a RPC JSON result into a strongly typed obejct, update the provided IWallet, and handle the result. This triggers an event providing full insight into the process and results. The 3rd boolean arguement will fire the IWallet's 'Update' event to let any wallet subscribers know of change. Notice how the 'AliasResponse' object type matches both in the class deinfition and as the result being passed to the HandleResult() function from the base class.
