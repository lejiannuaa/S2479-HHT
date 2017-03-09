package com.hola.bs.hhtserver.server;

import java.util.Date;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Qualifier;

import com.hola.bs.impl.JdbcTemplateUtil;
import com.hola.bs.json.detailVO.StkPlanInfo;
import com.hola.bs.property.SQLPropertyUtil;
import com.hola.bs.util.DateUtils;
import com.hola.bs.util.JsonUtil;

/**
 * 远程让HHTSERVER调用方法的实现
 * @author S2139
 * 2012 Oct 11, 2012 6:03:17 PM 
 */
public class RemoteServiceImpl implements RemoteServiceInterface {

	@Autowired(required = true)
	@Qualifier("jdbcTemplateUtil")
	private JdbcTemplateUtil jdbcTemplateUtil;
	
	@Autowired(required = true)
	private SQLPropertyUtil sqlpropertyUtil;
	
	public String test() {
		// TODO Auto-generated method stub
		return "This is the first remote method";
	}

	public String uploadStkPlanToBridgeServer(String json) throws Exception {
		// TODO Auto-generated method stub
		com.alibaba.fastjson.JSONObject jsonObject = JsonUtil.analyze(json);
		StkPlanInfo[] detail = JsonUtil.getDetail(jsonObject, StkPlanInfo.class);

		String sto = detail[0].getStoNo();
		String date = DateUtils.date2String2(new Date());
		String[] sqls = new String[detail.length];
		Object[] o = new Object[detail.length];
		
		for(int i=0; i<detail.length; i++){
			sqls[i] = sqlpropertyUtil.getValue("hht"+sto, "remote.stkplan.refresh");
			o[i] = new Object[]{detail[i].getStkNo(),detail[i].getStoNo(),detail[i].getStkDate(),
					detail[i].getStkType(),detail[i].getStkStatus(),detail[i].getStkChanger(),detail[i].getStkComment(),detail[i].getStkOprUser(),date};
		}
		int ret = jdbcTemplateUtil.update(sqls, o);
		
		return ""+ret;
	}

}
