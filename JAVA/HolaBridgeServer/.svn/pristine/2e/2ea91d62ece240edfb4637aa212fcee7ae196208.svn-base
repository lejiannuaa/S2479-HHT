package com.hola.bs.service.hht;

import java.util.Date;
import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.remoting.RemoteLookupFailureException;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.json.detailVO.SkuQueryDto;
import com.hola.bs.print.rmi.PrintServer;
import com.hola.bs.print.template.PrintTemplate9;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.DateUtils;
import com.hola.bs.util.JsonUtil;

public class HHT_1046_02 extends BusinessService implements ProcessUnit  {
	
	
	@Autowired(required = true)
	private PrintTemplate9 printTemplate9;
	
	public PrintTemplate9 getPrintTemplate9() {
		return printTemplate9;
	}

	public void setPrintTemplate9(PrintTemplate9 printTemplate9) {
		this.printTemplate9 = printTemplate9;
	}

	private PrintServer printServer;
	
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
			throw new RuntimeException();
		}
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getRequest().getParameter("sto"));
		log.info("下单申请-提交, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) {
		// TODO Auto-generated method stub
		
		String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		String today = DateUtils.date2String(new Date());
		com.alibaba.fastjson.JSONObject jsonObject = JsonUtil.analyze(bean.getRequest().getParameter(configpropertyUtil.getValue("json")));
		SkuQueryDto[] tSkuQueryDto = JsonUtil.getDetail(jsonObject,SkuQueryDto.class);
		String sysId = "";
		if (bean.getUser() != null) {
			sysId = store + this.systemUtil.getSysid();
		} else {
			sysId = this.systemUtil.getSysid();
		}
		String time = DateUtils.string2TotalTime(new Date());
		if(tSkuQueryDto!=null){
			int no = 0;
			String[] batchSqls = new String[tSkuQueryDto.length];
			Object[] o = new Object[tSkuQueryDto.length];
			for(SkuQueryDto skuQueryDto:tSkuQueryDto){
				batchSqls[no] = sqlpropertyUtil.getValue(store, "hht1046.02.01");
				o[no] = new Object[]{store,skuQueryDto.getSku(),skuQueryDto.getSku_dsc(),skuQueryDto.getStk_order_qty(),bean.getUser().getName(),time,"Y",sysId};
				no++;
			}
			jdbcTemplateUtil.update(batchSqls, o);
		}
		String printSql = sqlpropertyUtil.getValue(store, "hht1046.02.02");
		
		List<Map<String, Object>> detailMapPrintList = jdbcTemplateUtil.searchForList(printSql, new Object[]{store,sysId,bean.getUser().getName()});
		
		try {
			printServer = super.createPrintClient(bean.getUser().getStore());
			String[] fileMettle = printTemplate9.createReportListSubmit(
					bean.getUser().getOwnerFilePath(), 
					store,
					new String[]{store,today,bean.getUser().getName()},detailMapPrintList);
			
			for(int i=0;i<fileMettle.length;i++){
				printServer.print(bean.getUser().getName()+"\\"+fileMettle[i]);
			}

			Thread.sleep(1000);
		}catch(RemoteLookupFailureException rlfe){
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("调用远程服务失败，请检查店铺"+ bean.getUser().getStore()+"的网络线路状况以及打印服务是否正常启动。");
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
}
