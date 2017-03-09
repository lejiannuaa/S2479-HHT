package com.hola.bs.service.hht;

import java.math.BigDecimal;
import java.util.Arrays;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;
import org.springframework.beans.factory.annotation.Autowired;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.json.detailVO.SkuQueryDto;
import com.hola.bs.print.template.PrintTemplate11;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.Config;
import com.hola.bs.util.DateUtils;
import com.hola.bs.util.JsonUtil;
import com.hola.bs.util.XmlElement;

public class HHT_1045_02 extends BusinessService implements ProcessUnit {
	
	@Autowired(required = true)
	private PrintTemplate11 printTemplate11;

	public PrintTemplate11 getPrintTemplate11() {
		return printTemplate11;
	}

	public void setPrintTemplate11(PrintTemplate11 printTemplate11) {
		this.printTemplate11 = printTemplate11;
	}

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
		log.info("店店调拨-提交, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}
	
	private void processData(BusinessBean bean) throws Exception{
		// TODO Auto-generated method stub
		String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
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
			HashMap<String, String> tmap = new HashMap<String, String>();
			String today = DateUtils.date2String(new Date());
			for(SkuQueryDto skuQueryDto:tSkuQueryDto){
				batchSqls[no] = sqlpropertyUtil.getValue(store, "hht1045.02.01");
				o[no] = new Object[]{store,skuQueryDto.getSto(),skuQueryDto.getSku(),skuQueryDto.getStk_order_qty(),bean.getUser().getName(),time,"Y",sysId};
				no++;
				tmap.put(skuQueryDto.getSto(), skuQueryDto.getSto());
				
			}
			jdbcTemplateUtil.update(batchSqls, o);
			
			for(Map.Entry<String, String> entry : tmap.entrySet()){
				String execlSql = sqlpropertyUtil.getValue(store, "hht1045.02.02");
				List<Map<String, Object>> execlList = jdbcTemplateUtil.searchForList(execlSql, new Object[]{sysId,entry.getValue()});
				
				if (execlList!=null&&execlList.size()>0){
					
					for(int i=0;i<execlList.size();i++){
						String sqlSom = sqlpropertyUtil.getValue(store, "hht1045.02.03");
						Object[] oSom = new Object[]{entry.getValue(),execlList.get(i).get("sku").toString()};
						sqlSom = replaceSqlPram(sqlSom,oSom);
						List<Map> storageList = jdbcTemplateSomUtil.searchForList(sqlSom);
						if(storageList!=null&&storageList.size()>0){
							execlList.get(i).put("outhan", ((BigDecimal)storageList.get(0).get("HHTHAN")).intValue());
						}
						
					}
				String path = configpropertyUtil.getValue("EXECL_EXPORT_FOLDER") + "\\" + today + "\\" + bean.getUser().getName() + "\\";
					String fileMettle = printTemplate11.creatReports(
							path, 
							store,
							execlList,today,bean.getUser().getName(),entry.getValue());
					
				}
				
				
				
			}
			
		}
		
	}


}
