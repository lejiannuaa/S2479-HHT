package com.hola.bs.service.hht;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.bean.JsonBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.JsonUtil;

/**
 * 保存实际出货的商品箱明细数量
 * @author S2139
 * 2012 Sep 6, 2012 2:15:12 PM 
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_204_02 extends BusinessService implements ProcessUnit {

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
		return bean.getResponse().toString();

	}

	private void processData(BusinessBean bean) throws Exception{
		// TODO Auto-generated method stub
		String bc = bean.getRequest().getParameter(configpropertyUtil.getValue("bc"));
//		String jsonStr = bean.getRequest().getParameter(configpropertyUtil.getValue("json"));
		String store = bean.getUser()!=null?bean.getUser().getStore():"13101";
		
		List<String> l = new ArrayList<String>();
		l.add(configpropertyUtil.getValue("hht001nodeName.01"));
		// l.add(configpropertyUtil.getValue("hht001nodeName.02"));
		JsonBean json = JsonUtil.jsonToList(String.valueOf(bean.getRequest().getParameter(
				configpropertyUtil.getValue("xml"))), l);
		Map data[] = json.getData().get(configpropertyUtil.getValue("hht001nodeName.01"));
		String[] updateSqls = new String[data.length];
		Object[] o = new Object[data.length];
		int no = 0;
		for(Map m : data){
			updateSqls[no] = sqlpropertyUtil.getValue(store, "hht204.01.02");
			o[no]=new Object[]{String.valueOf(m.get(configpropertyUtil.getValue("thsl"))),
					bc,
					String.valueOf(m.get(configpropertyUtil.getValue("sku"))),
					String.valueOf(m.get(configpropertyUtil.getValue("xh")))};
			no++;
		}
		String sysId = "";
		if (bean.getUser() != null) {
			sysId = bean.getUser().getStore() + this.systemUtil.getSysid();
		} else {
			sysId = this.systemUtil.getSysid();
		}
		String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
		String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
		super.updateForValidation(updateSqls, o, bean.getUser().getStore(), guid, requestValue, "");
	//	this.jdbcTemplateUtil.update(updateSqls, o);
	}

}
