package com.hola.bs.print.template;

import java.util.List;
import java.util.Map;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;

import com.hola.bs.print.PrintConfigUtil;

public class PrintTemplate11 extends PrintTemplate{
	
	private static Logger logger = LoggerFactory
			.getLogger(PrintTemplate10.class);

	public PrintTemplate11() {
	}

	public PrintTemplate11(JdbcTemplate jdbcTemplate) {
		super(jdbcTemplate);
	}

	@Override
	public String createReport(String path, String storeid, String[] sqlParams,
			String... detailsqlParams) throws Exception {
		// TODO Auto-generated method stub
		return null;
	}
	
	public String creatReports(String path, String storeid, List<Map<String, Object>> execlList,
			String... detailsqlParams) throws Exception {
		// TODO Auto-generated method stub
		
		String headerSql = PrintConfigUtil.getTemplate11headerSql(storeid);
		logger.info("表头SQL:{}",headerSql);
		logger.info("参数=" + detailsqlParams[2]);
		
		List<Map<String, Object>> headerMapList = jdbcTemplate.queryForList(
				headerSql, new String[]{storeid,detailsqlParams[2]});
		
		if (headerMapList.size() == 0) {
			throw new Exception("未取得调拨出货单表头信息");
		}

		Map<String, Object> headerMap = headerMapList.get(0);
		
		headerMap.put("applyname",detailsqlParams[1]);
		headerMap.put("applyday", detailsqlParams[0]);
		
		return createReportExecl(path, wb, headerMap, execlList, this.getTemplateName());
	}

	@Override
	public String getTemplateName() {
		// TODO Auto-generated method stub
		return PrintConfigUtil.getTemplate11Name();
	}

	@Override
	public String getPrinterName() {
		// TODO Auto-generated method stub
		return PrintConfigUtil.getTemplate11PrinterName();
	}

}
