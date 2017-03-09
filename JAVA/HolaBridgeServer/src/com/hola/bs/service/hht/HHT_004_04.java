package com.hola.bs.service.hht;

import java.util.List;
import java.util.Map;

import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.property.PullDownUtils;
import com.hola.bs.service.BusinessService;

/**
 * 顯示PO收貨頭部信息的數據檢索
 * @author s1713 modify s2139
 *
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_004_04  extends BusinessService implements ProcessUnit {
	


	public String process(Request request) {
		BusinessBean bean=new BusinessBean();
		try {
			 bean=resolveRequest(request);
			processData(bean);
		} catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。"+e.getMessage());
			log.error("", e);
			throw new RuntimeException();
		}
		return  bean.getResponse().toString();
	}
	
	
	

	private void processData(BusinessBean bean) throws Exception{
		
		
		String bc = bean.getRequest().getParameter(configpropertyUtil.getValue("bc"));
		while (bc.length() < 6) {
			bc = "0" + bc;
		}
			List<Map> list=jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht004.03"), new Object[]{bc});
			
			String userName = bean.getUser().getName();
			String s="";
			if(list.size()<1){
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc(s+configpropertyUtil.getValue("msg.04"));
			}else{
				s=String.valueOf(list.get(0).get("D1STAT"));
				if(s!=null&&"3".equals(s.trim())){
					String sql[]=new String[]{ sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht004.04.01")};
					Object o[]=new Object[1];
					o[0]=new Object[]{this.getSystemUtil().getSysid(), userName,bc};
					
					String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
					String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
					super.updateForValidation(sql, o, bean.getUser().getStore(), guid, requestValue, "");
					//this.jdbcTemplateUtil.update(sql, o);
				}else{
					bean.getResponse().setCode(BusinessService.errorcode);
					bean.getResponse().setDesc("PO单"+PullDownUtils.getHHTStatusList().get(s)+"下不可进入收货");	
				}
				
			}
		
	}
}
