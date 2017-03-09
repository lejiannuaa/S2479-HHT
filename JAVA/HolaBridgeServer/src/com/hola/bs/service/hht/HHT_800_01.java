package com.hola.bs.service.hht;

import java.net.InetAddress;
import java.util.Date;

import org.apache.log4j.MDC;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.json.detailVO.SktTestDetail;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.DateUtils;
import com.hola.bs.util.JsonUtil;

/**
 * HHT检测数据上传
 * @author s2139
 * 2012 Aug 28, 2012 3:08:10 PM 
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_800_01 extends BusinessService implements ProcessUnit {

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
		MDC.put("stoNo", bean.getRequest().getParameter(configpropertyUtil.getValue("sto")));
		log.info("测试与服务器通信能力, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception{
		// TODO Auto-generated method stub
		String userId = bean.getUser()!=null?bean.getUser().getName():"s2139";
		String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		String sn = bean.getRequest().getParameter(configpropertyUtil.getValue("sn"));
		String bridgeServerIp = InetAddress.getLocalHost().getHostAddress().toString();
		String hhtIp = bean.getRequest().getParameter(configpropertyUtil.getValue("hhtIp"));
		String date = DateUtils.date2String2(new Date());
		
		com.alibaba.fastjson.JSONObject jsonObject = JsonUtil.analyze(bean.getRequest().getParameter(configpropertyUtil.getValue("json")));
		SktTestDetail[] stds = JsonUtil.getDetail(jsonObject, SktTestDetail.class);
		String[] sqls = new String[stds.length];
		Object[] o = new Object[stds.length];
		int no = 0;
		for(SktTestDetail s : stds){
			sqls[no] = sqlpropertyUtil.getValue(store, "hht800.01.01");
			o[no] = new Object[]{store,sn,s.getLoc_no(),s.getSku(),s.getSku_dsc(),s.getSku_test_qty(),bridgeServerIp,hhtIp,userId,date};
			no++;
		}
		
		int ret = jdbcTemplateUtil.update(sqls, new Object[]{o});
	}

}
