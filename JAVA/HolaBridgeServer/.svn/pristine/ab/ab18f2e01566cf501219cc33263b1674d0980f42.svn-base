package com.hola.bs.print.template;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;

import com.hola.bs.print.PrintConfigUtil;
import com.hola.bs.util.DateUtils;

public class PrintTemplate10 extends PrintTemplate {

	private static Logger logger = LoggerFactory
			.getLogger(PrintTemplate10.class);

	public PrintTemplate10() {
	}

	public PrintTemplate10(JdbcTemplate jdbcTemplate) {
		super(jdbcTemplate);
	}

	@Override
	public String createReport(String path, String storeid, String[] sqlParams,
			String... detailsqlParams) throws Exception {
		// TODO Auto-generated method stub
		return null;
	}

	public String[] createReportList(String path, String storeid,
			String[] sqlParams, String... detailsqlParams) throws Exception {

		String headerSql = PrintConfigUtil.getTemplate10headerSql(storeid);
		logger.info("表头SQL:{}",headerSql);
		String detailSql = PrintConfigUtil.getTemplate10detailSql(storeid);
		logger.info("明細SQL:{}",detailSql);
		List<Map<String, Object>> headerMapList = jdbcTemplate.queryForList(
				headerSql, sqlParams);

		if (headerMapList.size() == 0) {
			throw new Exception("未取得调拨出货单表头信息");
		}

		
		List<Map<String, Object>> detailMapList = jdbcTemplate.queryForList(
				detailSql, detailsqlParams);
        Double totalmoney;
		String[] list = new String[detailMapList.size() / 40 + 1];

		for (int i = 0; i < list.length; i++) {
			totalmoney = 0D;
			List<Map<String, Object>> tDetailMapList = new ArrayList<Map<String, Object>>();
			for (int j = 0; j < 40; j++) {
				if (i != 0 && (40 * i + j) < detailMapList.size()) {
					totalmoney = totalmoney + ((BigDecimal)detailMapList.get(j).get("money")).doubleValue();
					tDetailMapList.add(detailMapList.get(40 * i + j));
				} else if (i == 0 && j< detailMapList.size()) {
					totalmoney = totalmoney + ((BigDecimal)detailMapList.get(j).get("money")).doubleValue();
					tDetailMapList.add(detailMapList.get(j));
				}
			}
			
			Map<String, Object> headerMap = headerMapList.get(0);
			headerMap.put("totalmoney", totalmoney);
			list[i] = createReport(path, wb, headerMap, tDetailMapList,
					this.getTemplateName());
			Thread.sleep(50);
		}
		return list;

	}
	
	
	public String[] createReportListSubmit(String path, String storeid,
			String[] sqlParams, List<Map<String, Object>> detailMapList) throws Exception {

		String headerSql = PrintConfigUtil.getTemplate10headerSql(storeid);
		logger.info("表头SQL:{}",headerSql);

		List<Map<String, Object>> headerMapList = jdbcTemplate.queryForList(
				headerSql, sqlParams);

		if (headerMapList.size() == 0) {
			throw new Exception("未取得调拨出货单表头信息");
		}

		Map<String, Object> headerMap = headerMapList.get(0);

		String[] list = new String[detailMapList.size() / 40 + 1];

		for (int i = 0; i < list.length; i++) {
			List<Map<String, Object>> tDetailMapList = new ArrayList<Map<String, Object>>();
			for (int j = 0; j < 40; j++) {
				if (i != 0 && (40 * i + j) < detailMapList.size()) {

					tDetailMapList.add(detailMapList.get(40 * i + j));
				} else if (i == 0 && j< detailMapList.size()) {

					tDetailMapList.add(detailMapList.get(j));
				}
			}

			list[i] = createReport(path, wb, headerMap, tDetailMapList,
					this.getTemplateName());
			Thread.sleep(50);
		}
		return list;

	}

	@Override
	public String getTemplateName() {
		// TODO Auto-generated method stub
		return PrintConfigUtil.getTemplate10Name();
	}

	@Override
	public String getPrinterName() {
		// TODO Auto-generated method stub
		return PrintConfigUtil.getTemplate10PrinterName();
	}

}
