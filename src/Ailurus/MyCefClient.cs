using System;
using Xilium.CefGlue;

namespace Ailurus
{
    public class MyCefClient : CefClient
    {
        protected override CefLifeSpanHandler GetLifeSpanHandler()
        {
            return new MyCefLifeSpanHandler();
        }
    }

    internal class MyCefLifeSpanHandler : CefLifeSpanHandler
    {
        protected override void OnAfterCreated(CefBrowser browser)
        {
            base.OnAfterCreated(browser);
        }
    }
}
