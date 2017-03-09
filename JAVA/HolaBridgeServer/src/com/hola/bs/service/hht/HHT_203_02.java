package com.hola.bs.service.hht;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.remoting.RemoteLookupFailureException;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.bean.JsonBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;
import com.hola.bs.service.hht.strategy_HHT_203_02.Context;
import com.hola.bs.service.hht.strategy_HHT_203_02.PO_Process_HHT_203_02;
import com.hola.bs.service.hht.strategy_HHT_203_02.Transfer_Process_HHT_203_02;
import com.hola.bs.util.JsonUtil;
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_203_02 extends BusinessService implements ProcessUnit{
	
	//PO差异报表
//	private PrintTemplate1 printTemplate1;
	
	//PO收货报表
//	private PrintTemplate2 printTemplate2;
	
	//调拨差异
//	private PrintTemplate3 printTemplate3;
	
	//调拨收货
//	private PrintTemplate4 printTemplate4;
	
	//PO差异/收货报表处理 策略方式实现
	@Autowired(required=true)
	private PO_Process_HHT_203_02 po_process_hht_203_02;
	
	//调拨差异/收货报表处理 策略方式实现
	@Autowired(required=true)
	private Transfer_Process_HHT_203_02 transfer_process_hht_203_02;
//	private PrintServer printServer;
	
	public String process(Request request) {
		BusinessBean bean = new BusinessBean();
		try {
			bean = resolveRequest(request);
//			System.out.println("receive print command");
			processData(bean);
		}catch(RemoteLookupFailureException rlfe){
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("调用远程服务失败，请检查店铺"+ bean.getUser().getStore()+"的网络线路状况以及打印服务是否正常启动。");
//			log.error("", rlfe);
			throw new RuntimeException();
		}catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。" + e.getMessage());
//			e.printStackTrace();
			log.error("", e);
			throw new RuntimeException();
		}
//		log.info("response=" + bean.getResponse().toString());
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("operation hht打印调拨或PO收货报表, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception {
		String store = "";
		if (bean.getUser() != null) {
			store = bean.getUser().getStore();
		} else {
			store = "13101";
		}
		
		List<String> nodeNameList = new ArrayList<String>();
		nodeNameList.add("info");
		nodeNameList.add("detail");
		JsonBean json=JsonUtil.jsonToList(String.valueOf(bean.getRequest().getParameter("json")), nodeNameList);		
		Map data[] = json.getData().get("detail");
		Map type[] = json.getData().get("info");
		Context ctx_po = new Context(po_process_hht_203_02, super.createPrintClient(store));
		Context ctx_transfer = new Context(transfer_process_hht_203_02,super.createPrintClient(store));
		//调拨类型(TRF)
		if(type[0].get("type").equals("1")){
//			for(int i=0;i<data.length;i++){
//				Map datamap = data[i];
//				String po = String.valueOf(datamap.get("OddNum"));
//				String file1 = printTemplate3.createReport(bean.getUser().getOwnerFilePath(), bean.getUser().getStore(),
//						new String[] { po }, po);
//				printServer.print(bean.getUser().getName() + "\\" + file1);
//				
//				String file2 = printTemplate4.createReport(bean.getUser().getOwnerFilePath(), bean.getUser().getStore(),
//						new String[] { po }, po);
//				printServer.print(bean.getUser().getName() + "\\" + file2);
//			}
			ctx_transfer.executePrint(data, bean);
		}else if(type[0].get("type").equals("0")){//PO类型
//			for(int i=0;i<data.length;i++){
//				Map datamap = data[i];
//				String po = String.valueOf(datamap.get("OddNum"));
//				String file1 = printTemplate1.createReport(bean.getUser().getOwnerFilePath(), bean.getUser().getStore(),
//						new String[] { po }, po);
//				printServer.print(bean.getUser().getName() + "\\" + file1);
//				
//				String file2 = printTemplate2.createReport(bean.getUser().getOwnerFilePath(), bean.getUser().getStore(),
//						new String[] { po }, po);
//				printServer.print(bean.getUser().getName() + "\\" + file2);
//			}
			ctx_po.executePrint(data, bean);
		}
		
		
//		for(int i=0;i<data.length;i++){
//			Map datamap = data[i];
//			sku = String.valueOf(datamap.get(configpropertyUtil.getValue("SKU")));
//			if (datamap.get(configpropertyUtil.getValue("实际出货量")) != null)
//					reallycount = Integer.parseInt(datamap.get(configpropertyUtil.getValue("实际出货量")).toString());
//			else throw new Exception();
//			sql[i] = sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht202.02");
//			o[i] = new Object[] {reallycount, bc, sku};
//		}

//		this.jdbcTemplateUtil.update(sql, o );

	}
}
