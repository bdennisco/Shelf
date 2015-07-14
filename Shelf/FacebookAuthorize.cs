using Shelf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Shelf
{
    public class FacebookAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            string accessToken = null;
            FacebookUser me = null;

            try {
                // Check session for access token
                if (httpContext.Session["access_token"] != null){
                    accessToken = httpContext.Session["access_token"].ToString();
                } else {
                    // Check identity for access token
                        //Get the current claims principal
                        var identity = (ClaimsPrincipal)System.Threading.Thread.CurrentPrincipal;
                        var claims = identity.Claims;
                        // Access claims
                        foreach (Claim claim in claims)
                        {
                            if (claim.Type == "FacebookAccessToken")
                            {
                                accessToken = claim.Value;
                            }
                        }
                }

                if (accessToken == null)
                {
                    return false;
                }
                // End access token section

                // Check session for Me
                if ((FacebookUser)httpContext.Session["me"] != null){
                    me = (FacebookUser)httpContext.Session["me"];
                } else {
                    // Get new Facebook data with access token
                    Facebook.FacebookClient facebook = new Facebook.FacebookClient(accessToken.ToString());
                    dynamic RawMe = facebook.Get("me?fields=name,picture.type(square)");
                    dynamic RawFriends = facebook.Get("me/friends?fields=name,picture.type(square),installed");

                    // Use Raw Data to populate helpful objects
                    me = new FacebookUser(RawMe, RawFriends.data);

                    // Put Me in session
                    httpContext.Session["me"] = me;
                }
                // End Me section
            }
            catch (Exception e) {
                return false;
            }

            if (me != null)
            {
                me = null;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}