package com.hola.bs.service.hht;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.remoting.RemoteLookupFailureException;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.print.rmi.PrintServer;
import com.hola.bs.print.template.PrintTemplate6;
import com.hola.bs.service.BusinessService;

public class HHT_106_03 extends BusinessService implements ProcessUnit{
	
	@Autowired(required = true)
	private PrintTemplate6 printTemplate6;
	
	private PrintServer printServer;
	
	public String process(Request request) {
		BusinessBean bean = new BusinessBean();
		try {
			bean = resolveRequest(request);
			//查询单据下所有的箱号
			String bc = request.getParameter("bc");
			List<String> list = getHHTCNO(bean.getUser().getStore(), bc);
			for(String hhtcno : list){
				String file = printTemplate6.createReport(bean.getUser().getOwnerFilePath(), bean.getUser().getStore(),new String[]{hhtcno}, hhtcno);
//				System.out.println("=========="+bean.getUser().getName()+"\\"+file);
				printServer = super.createPrintClient(bean.getUser().getStore());
				printServer.print(bean.getUser().getName()+"\\"+file);
			}
		}catch(RemoteLookupFailureException rlfe){
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("调用远程服务失败，请检查店铺"+ bean.getUser().getStore()+"的网络线路状况以及打印服务是否正常启动。");
//			log.error("", rlfe);
		}catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。" + e.getMessage());
			log.error("", e);
		}
	//	log.info("response=" + bean.getResponse().toString());
		
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("operation hht106.03打印箱明细单, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}
	
	private List<String> getHHTCNO(String store, String hhtno){
		
		List map = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(store,
		"hht104.04.01"), new Object[] {hhtno});
		
		List<String> list = new ArrayList<String>();
		for(int i=0;i<map.size();i++){
			if(map.get(i)!=null)
				list.add(((Map)map.get(i)).get("HHTCNO").toString());
		}
		
		return list;
	}

	
}
