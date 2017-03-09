package com.hola.bs.service.hht;

import java.math.BigDecimal;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.remoting.RemoteLookupFailureException;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.print.template.PrintTemplate11;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.DateUtils;

public class HHT_1045_05 extends BusinessService implements ProcessUnit {

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

			String store = bean.getRequest().getParameter(
					configpropertyUtil.getValue("sto"));
			String startTime = bean.getRequest().getParameter(
					configpropertyUtil.getValue("starttime"));
			String today = DateUtils.date2String(new Date());

			String sql = sqlpropertyUtil.getValue(store, "hht1045.04.01");
			Object[] o = new Object[] { today, startTime,
					bean.getUser().getName() };

			List<Map> gridList = jdbcTemplateUtil.searchForList(sql, o);
			String sysId = "";
			if (gridList != null && gridList.size() > 0) {

				HashMap<String, String> tmap = new HashMap<String, String>();

				for (int i = 0; i < gridList.size(); i++) {

					tmap.put(gridList.get(i).get("sto_no_out").toString(),
							gridList.get(i).get("sto_no_out").toString());

					sysId = gridList.get(i).get("instno").toString();

					

				}
				
				for (Map.Entry<String, String> entry : tmap.entrySet()) {
					String execlSql = sqlpropertyUtil.getValue(store,
							"hht1045.02.02");
					List<Map<String, Object>> execlList = jdbcTemplateUtil
							.searchForList(execlSql, new Object[] { sysId,
									entry.getValue() });

					if (execlList != null && execlList.size() > 0) {

						for (int i = 0; i < execlList.size(); i++) {
							String sqlSom = sqlpropertyUtil.getValue(store,
									"hht1045.02.03");
							Object[] oSom = new Object[] {
									entry.getValue(),
									execlList.get(i).get("sku").toString() };
							sqlSom = replaceSqlPram(sqlSom, oSom);
							List<Map> storageList = jdbcTemplateSomUtil
									.searchForList(sqlSom);

							execlList.get(i).put(
									"outhan",
									((BigDecimal) storageList.get(0).get(
											"HHTHAN")).intValue());
						}

						String fileMettle = printTemplate11.creatReports(
								configpropertyUtil
										.getValue("EXECL_EXPORT_FOLDER"),
								store, execlList, today, bean.getUser()
										.getName(), entry.getValue());

					}
				}

			}

		} catch (Exception e) {
			// TODO Auto-generated catch block
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。" + e);
			log.error("", e);
		}
		return bean.getResponse().toString();

	}

}
