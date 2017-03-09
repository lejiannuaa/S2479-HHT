package com.hola.bs.service.hht;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;
import org.json.JSONObject;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;

public class HHT_501_03 extends BusinessService implements ProcessUnit{

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
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getRequest().getParameter(configpropertyUtil.getValue("sto")));
		log.info("保存初盘数据, response="+bean.getResponse().toString());
		System.out.println("放弃该柜号盘点作业, response="+bean.getResponse().toString());
		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception {
		// TODO Auto-generated method stub
		String userId = bean.getUser() != null?bean.getUser().getName():"S2479";
		String stk_no = bean.getRequest().getParameter(configpropertyUtil.getValue("stk_no"));
		String loc_no = bean.getRequest().getParameter(configpropertyUtil.getValue("loc_no"));
		String sto = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		Object[] pra = new Object[]{stk_no,userId,loc_no};

		List<Map<String,String>> list = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(sto,"hht500.00.03"), pra);
		if(list!=null&&list.size()>0){
			String loc_status = list.get(0).get("loc_status");
			
			if(!loc_status.equals("O"))
			{
				String strUrl = configpropertyUtil.getValue("SOM_URL");
				strUrl = strUrl + stk_no + "&stono=" + sto + "&countType=first_count_loc&stkLocno=" + loc_no +"&status=Y";
		        URL url = new URL(strUrl);
				
				HttpURLConnection httpconn = (HttpURLConnection) url.openConnection();
				InputStreamReader input = new InputStreamReader(httpconn.getInputStream(),"utf-8");
				BufferedReader bufReader = new BufferedReader(input);
				
				JSONObject resultJsonObject = new JSONObject(bufReader.readLine());
				if(resultJsonObject.get("state").equals("success")){
					String[] sql = new String[1];
					
					sql[0] = sqlpropertyUtil.getValue(sto,"hht501.03.01");
					
					Object[] o = new Object[1];
					o[0]=new Object[]{ sto,stk_no,loc_no };
					jdbcTemplateUtil.update(sql, o);
				}else{
					//bean.getResponse().setCode(BusinessService.errorcode);
					//bean.getResponse().setDesc(configpropertyUtil.getValue("msg.501.02"));
				}
			}
		}
	}
	
}
