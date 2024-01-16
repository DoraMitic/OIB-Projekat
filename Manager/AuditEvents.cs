using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;

namespace Manager
{
	public enum AuditEventTypes
	{
		AuthenticationSuccess = 0,
		AuthorizationSuccess = 1,
		AuthorizationFailed = 2,
		AuthenticationFailed = 3,
		TransactionSuccess = 4,
		TransactionFailed = 5
	}

	public class AuditEvents
	{
		private static ResourceManager resourceManager = null;
		private static object resourceLock = new object();

		private static ResourceManager ResourceMgr
		{
			get
			{
				lock (resourceLock)
				{
					if (resourceManager == null)
					{
						resourceManager = new ResourceManager
							(typeof(AuditEventFile).ToString(),
							Assembly.GetExecutingAssembly());
					}
					return resourceManager;
				}
			}
		}

		public static string AuthenticationSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.AuthenticationSuccess.ToString());
			}
		}

		public static string AuthorizationSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.AuthorizationSuccess.ToString());
			}
		}

		public static string AuthorizationFailed
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.AuthorizationFailed.ToString());
			}
		}

		public static string AuthenticationFailed
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.AuthenticationFailed.ToString());
			}
		}

		public static string TransactionSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.TransactionSuccess.ToString());
			}
		}

		public static string TransactionFailed
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.TransactionFailed.ToString());
			}
		}
	}
}
