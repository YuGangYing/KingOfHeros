using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System;
using LitJson;
//-
namespace Login
{
	public class IggLogin
	{
        bool bInit = true;//false;

		const string c_strPubKey = "85e927bd9203be51dfb40e6f9d245252";
		const string c_strUrl = "http://cgi.igg.com:9000/public/guest_user_login_igg?";
		const string c_strGuest = "m_guest";
		const string c_strKey = "m_key";
		const string c_strData = "m_data";
		const string c_strKeeptime = "keep_time";
		const int 	 c_nKeeptime = 259200;
		const int 	 c_nTimeOut = 4;

		const string c_strErrmsg = "errStr";
		const string c_strErrcode = "errCode";
		const string c_strIggid = "iggid";
		const string c_strAccessKey = "access_key";
		const string c_strResult = "result";

		string m_strGuest = "123456";
		string m_strKey = "";
		string m_strData = "";
		string m_strErrmsg = "";
		public string m_strIggId = "12345";
        public string m_strToken = "12345";
		int    m_nErrcode;

		LoginResult m_callback = null;
		LOGIN_RET m_result;

		public IggLogin()
		{
		}

		public string getIggId()
		{
			return m_strIggId;
		}

		public string getToken()
		{
			return m_strToken;
		}

		public LOGIN_RET result()
		{
			return m_result;
		}

		public string getErrMsg()
		{
			return m_strErrmsg;
		}

		string getUrl()
		{
			string strUrl = c_strUrl;

			strUrl += c_strGuest;
			strUrl += "=";
			strUrl += m_strGuest;

			strUrl += "&";
			strUrl += c_strKey;
			strUrl += "=";
			strUrl += m_strKey;

			strUrl += "&";
			strUrl += c_strData;
			strUrl += "=";
			strUrl += m_strData;

			strUrl += "&";
			strUrl += c_strKeeptime;
			strUrl += "=";
			strUrl += c_nKeeptime.ToString();

			return strUrl;
		}

		public bool login(LoginResult ret)
		{
			this.m_callback = ret;
            if (!bInit)
            {
                this.m_result = LOGIN_RET.SUCC;
                this.m_callback(m_result,this.m_strErrmsg);
                return true;
            }
			//

            m_strGuest = UnityEngine.SystemInfo.deviceUniqueIdentifier;
			m_strKey = DateTime.Now.Ticks.ToString();
			m_strData = String2MD5(m_strGuest,m_strKey);

			string strUrl = this.getUrl();
			Debug.Log(strUrl);

			HttpWebRequest request = WebRequest.Create(strUrl) as HttpWebRequest;
			request.ContentType = "Accept-Encoding";
			request.Method = "GET";
			request.Timeout = c_nTimeOut;
			request.BeginGetResponse(new AsyncCallback(onRead),request);
			return true;
		}

		string String2MD5(string strGuest, string strKey)
		{
			MD5 md5Hash = MD5.Create();		
			string str = strGuest + c_strPubKey + strKey;
			
			byte[] BytesIn = Encoding.ASCII.GetBytes(str);
			byte[] BytesOut = md5Hash.ComputeHash(BytesIn);
			
			StringBuilder stringBuilder = new StringBuilder();
			for (int j = 0; j < BytesOut.Length; j++)
			{
				stringBuilder.Append(BytesOut[j].ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		void onRead(IAsyncResult result)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)result.AsyncState;
				HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
				Stream stream = response.GetResponseStream();
				StreamReader reader = new StreamReader(stream);
				string strResult = reader.ReadToEnd();

				parseResult(strResult);
			}
			catch(Exception e)
			{
				this.m_result = LOGIN_RET.FAILED;
				this.m_strErrmsg = e.Message;
			}
			finally
			{
				if(this.m_callback!=null)
					this.m_callback(this.m_result,this.m_strErrmsg);
			}
		}

		bool parseResult(string strResult)
		{
			Debug.Log(strResult);
			int nEnd = strResult.LastIndexOf("}");
			string strJson = strResult.Substring(0,nEnd+1);
			//json
			try
			{
				JsonData json = JsonMapper.ToObject(strJson);
				if(json!=null)
				{
					this.m_nErrcode = (int)json[c_strErrcode];
					this.m_strErrmsg = (string)json[c_strErrmsg];
					if(this.m_nErrcode!=0)
						return false;
					JsonData item = json[c_strResult]["0"];
					this.m_strIggId = (string)item[c_strIggid];
					this.m_strToken = (string)item[c_strAccessKey];
					this.m_result = LOGIN_RET.SUCC;
					return true;
				}
				this.m_result = LOGIN_RET.FAILED;
				this.m_strErrmsg = "Json parse failed!";
				return false;
			}
			catch(Exception e)
			{
				Debug.Log(e.Message);
				this.m_result = LOGIN_RET.FAILED;
				this.m_strErrmsg = "Json parse failed!";
				return false;
			}
		}
	}
}