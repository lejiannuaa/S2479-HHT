package com.hola.bs.service.hht;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;
import org.json.JSONObject;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;

public class HHT_501_02 extends BusinessService implements ProcessUnit{

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
		System.out.println("初盘状态更变为盘点中, response="+bean.getResponse().toString());
		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception {
		// TODO Auto-generated method stub
		String stk_no = bean.getRequest().getParameter(configpropertyUtil.getValue("stk_no"));
		String loc_no = bean.getRequest().getParameter(configpropertyUtil.getValue("loc_no"));
		String sto = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		String userNo = bean.getUser().getName();
		String checkSql = sqlpropertyUtil.getValue(sto,"hht501.01.04");
		Object[] check = new Object[]{ stk_no,loc_no,userNo,sto };
		List<Map> checkList = jdbcTemplateUtil.searchForList(checkSql,check);

		if(checkList==null||checkList.size()==0){
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc(configpropertyUtil.getValue("msg.501.03"));
		}else{
			if(!checkList.get(0).get("STK_LOC_STATUS").equals("O"))
			{
				String strUrl = configpropertyUtil.getValue("SOM_URL");
				strUrl = strUrl + stk_no + "&stono=" + sto + "&countType=first_count_loc&stkLocno=" + loc_no +"&status=I";
		        URL url = new URL(strUrl);
				
				HttpURLConnection httpconn = (HttpURLConnection) url.openConnection();
				InputStreamReader input = new InputStreamReader(httpconn.getInputStream(),"utf-8");
				BufferedReader bufReader = new BufferedReader(input);
				
				JSONObject resultJsonObject = new JSONObject(bufReader.readLine());
				
				if(resultJsonObject.get("state").equals("success")){
					String[] sql = new String[1];
					
					sql[0] = sqlpropertyUtil.getValue(sto,"hht501.02.01");
					
					Object[] o = new Object[1];
					o[0]=new Object[]{ sto,stk_no,loc_no };
					jdbcTemplateUtil.update(sql, o);
				}else{
					//bean.getResponse().setCode(BusinessService.errorcode);
					//bean.getResponse().setDesc(configpropertyUtil.getValue("msg.501.02"));
				}
			}
			else
			{
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc("该柜号已完成盘点");
			}
		}
		
		
		
	}


}
