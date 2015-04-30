<Query Kind="Program">
  <Reference>C:\sample\Newtonsoft.Json.6.0.6\lib\net45\Newtonsoft.Json.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <Reference>C:\sample\Yandex.Money.Api.Sdk.1.0.0\lib\net45\Yandex.Money.Api.Sdk.dll</Reference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
  <Namespace>Yandex.Money.Api.Sdk</Namespace>
  <Namespace>Yandex.Money.Api.Sdk.Authorization</Namespace>
  <Namespace>Yandex.Money.Api.Sdk.Interfaces</Namespace>
  <Namespace>Yandex.Money.Api.Sdk.Net</Namespace>
  <Namespace>Yandex.Money.Api.Sdk.Requests</Namespace>
  <Namespace>Yandex.Money.Api.Sdk.Responses</Namespace>
  <Namespace>Yandex.Money.Api.Sdk.Utils</Namespace>
</Query>

// http://tech.yandex.com/money/doc/dg/tasks/register-client.xml
static String ClientId = "...";
static String RedirectUri = "...";

void Main()
{
	var form = new Form();
	
	label = new Label { Dock = DockStyle.Bottom};
	
	var panel = new Panel { Dock = DockStyle.Bottom };
	panel.Controls.Add(label);
	
	var browser = new WebBrowser { Dock = DockStyle.Fill };
	browser.Refresh();
	
	var wrapper = new Wrapper(browser);
	
	form.Load += (s, e) =>
		Task.Run (async () => 
		{
			var ab = new AuthorizationBroker();
			
			// Obtain OAuth2 access_token
						
			var token = await ab.AuthorizeAsync(dhpc, wrapper, hp.AuthorizationdUri.ToString(), query);
			
			auth.Token = token;
			
			// Show account information at the bottom of the panel
			
			var accountInfoRequest = new AccountInfoRequest<AccountInfoResult>(dhpc, new JsonSerializer<AccountInfoResult>());
	
			var accountInfoResult = await accountInfoRequest.Perform();
	
			label.Text = accountInfoResult.ToString();
		});
			
	form.Controls.Add(browser);		
	form.Controls.Add(panel);
	form.Show();
}

Label label;

private static IHostProvider hp = new DefaultHostsProvider();
private static Authenticator auth = new Authenticator();
IHttpClient dhpc = new DefaultHttpPostClient(hp, auth);

private static AuthorizationRequestParams query = new AuthorizationRequestParams 
		{
		    ClientId = ClientId,
		    RedirectUri = RedirectUri,
	    	Scope = Scopes.Compose(new[] { Scopes.AccountInfo })
		};
		

public class Authenticator: DefaultAuthenticator
{
	public override string Token { get; set;}
}



/*  following are wrappers to simplify work with the api */
		
public interface IWebBrowser<T>
{
	void Navigate(string uri, byte[] postParams, string additionalHeaders);
	event EventHandler<T> NavigationEvent;
}		

public class Wrapper: IWebBrowser<string>
{
	private WebBrowser _browser;

	public Wrapper(WebBrowser browser)
	{
		if (browser == null)
			return;
		
		_browser = browser;
		_browser.Navigated += (s, e) => { if (NavigationEvent != null) NavigationEvent(s, e.Url.Query); };	
	}
	
	public void Navigate(string uri, byte[] postParams, string additionalHeaders)
	{
		if (_browser != null)
			_browser.Navigate(uri, @"_self", postParams, additionalHeaders); 
	}
	
	public event EventHandler<string> NavigationEvent;
}

public class AuthorizationBroker
{
    private TaskCompletionSource<string> _tcs;
	
	public Task<string> AuthorizeAsync(IHttpClient client, IWebBrowser<string> browser, string uri, AuthorizationRequestParams prms)
	{
		if (browser == null)
			return null;
		
        _tcs = new TaskCompletionSource<string>();
		
		browser.NavigationEvent += async (s, e)=>
		{
			var d = Misc.QueryParamsToDictionary(e);
			
			if (d.ContainsKey("code"))
			{
			
				var tr = new TokenRequest<TokenResult>(client, new JsonSerializer<TokenResult>())
				{
   					Code = d["code"],
   					ClientId = prms.ClientId,
   					RedirectUri = prms.RedirectUri
				};
	
				var token = await tr.Perform();
				
				if (token == null)
					return;
			
		        if (_tcs != null)
        	        	_tcs.SetResult(token == null? null: token.Token);
			}
		};
		
		browser.Navigate(uri, prms.PostBytes(), @"Content-Type: application/x-www-form-urlencoded/r/n");

        return _tcs.Task;	
	}
}
