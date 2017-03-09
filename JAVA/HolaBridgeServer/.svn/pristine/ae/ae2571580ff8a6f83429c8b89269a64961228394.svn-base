package com.hola.bs.service.hht;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;

import org.apache.log4j.MDC;
import org.json.JSONObject;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;

public class HHT_501_04	extends BusinessService implements ProcessUnit{

		public String process(Request request) {
			// TODO Auto-generated method stub
			BusinessBean bean = new BusinessBean();
			try {
				bean = resolveRequest(request);
				processData(bean);
			} catch (Exception e) {
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc("系统错误，请联系管理员。" + e.getMessage());
				log.error("", e);
				throw new RuntimeException(e);
			}
			MDC.put("userNo", "SOM");
			MDC.put("stoNo", bean.getRequest().getParameter(configpropertyUtil.getValue("sto")));
			log.info("保存初盘数据, response="+bean.getResponse().toString());
			System.out.println("放弃该柜号盘点作业, response="+bean.getResponse().toString());
			return bean.getResponse().toString();
		}

		private void processData(BusinessBean bean) throws Exception {
			// TODO Auto-generated method stub

			String sto = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
			System.out.println(sto);
			
		}
}
