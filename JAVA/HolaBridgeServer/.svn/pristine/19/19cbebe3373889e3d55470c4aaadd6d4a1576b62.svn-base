package com.hola.bs.service.hht;

import java.util.Date;
import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.Config;
import com.hola.bs.util.DateUtils;
import com.hola.bs.util.XmlElement;

/**
 * 检核维护--查询
 * @author S2139
 * 2012 Aug 29, 2012 5:22:35 PM 
 */
public class HHT_1032_01 extends BusinessService implements ProcessUnit {

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
		}
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getRequest().getParameter("sto"));
		log.info("盘点检核柜号查询, response="+bean.getResponse().toString());

		return bean.getResponse().toString();

	}

	private void processData(BusinessBean bean)throws Exception {
		// TODO Auto-generated method stub
		String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		String loc_no = bean.getRequest().getParameter(configpropertyUtil.getValue("loc_no"));
		String usr = bean.getUser().getName();
		String dateStr = DateUtils.date2StringDate(new Date());
		
		StringBuffer sb = new StringBuffer();
		Object[] o = new Object[]{store,dateStr};
		String sql = sqlpropertyUtil.getValue(store, "hht1032.01.01");
		sb.append(sql);
		if(loc_no != null && loc_no.length()>0){
			sb.append(" and stk_locno = '").append(loc_no).append("'");
		}
		sb.append(" group by stk_locno ,stk_opr_time ");
		List<Map> list = jdbcTemplateUtil.searchForList(sb.toString(), o);

		if(list!=null&&list.size()>0){
			for(int i=0; i<list.size(); i++){
				Map map = list.get(i);
				String date = map.get("date").toString();
				date = DateUtils.date2String(DateUtils.string2Date(date, DateUtils.ID_YYYYMMDD));
				map.put("date", date);
			}
			Config c = new Config("32", "Server->Client：0", String.valueOf(bean.getRequest().getParameter("request"))
					+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
			XmlElement[] xml = new XmlElement[1];
			xml[0] = new XmlElement("detail", list);
			writerFile(c, xml, bean);
		}else{
			bean.getResponse().setCode(BusinessService.warncode);
			bean.getResponse().setDesc(configpropertyUtil.getValue("msg.700.02"));
		}
	}

	
}
