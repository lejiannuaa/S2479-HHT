package com.hola.bs.print.template;

import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.springframework.jdbc.core.JdbcTemplate;

import com.hola.bs.print.PrintConfigUtil;
import com.hola.bs.util.CommonStaticUtil;
import com.hola.bs.util.DateUtils;

public class PrintTemplate12 extends PrintTemplate{
	
	public PrintTemplate12() {
		// TODO Auto-generated constructor stub
	}
	
	public PrintTemplate12(JdbcTemplate jdbcTemplate) {
		// TODO Auto-generated constructor stub
		super(jdbcTemplate);
	}

	@Override
	public String createReport(String path, String storeid, String[] sqlParams,
			String... detailsqlParams) throws Exception {
		// TODO Auto-generated method stub
		
		String detailSql = PrintConfigUtil.getTemplate12detailSql(storeid);
		String headerSql = PrintConfigUtil.getTemplate10headerSql(storeid);

		List<Map<String, Object>> headerMapList = jdbcTemplate.queryForList(
				headerSql, storeid);
		Map<String, Object> headerMap = new HashMap<String, Object>();
		headerMap.put("applyday", DateUtils.date2String(new Date()));
		headerMap.put("store", headerMapList.get(0).get("sto_name").toString());
		
		List<Map<String, Object>> detailMapList = jdbcTemplate.queryForList(
				detailSql, sqlParams);
		double sumrtv = 0d;
		double sumtrf = 0d;
		
		double sumvol = 0D;
		double sumwei = 0D;
		double sumnum = 0D;
		
		
		for(Map<String, Object> detailMap : detailMapList){
			
			if(detailMap.get("HHTCNO").equals("PSP000001")){
				detailMap.put("HHTCNO", "PSP000001(破损品)");
			}else if(detailMap.get("HHTCNO").equals("JCP000001")){
				detailMap.put("HHTCNO", "JCP000001(寄仓品)");
			}else if(detailMap.get("HHTCNO").equals("WLX000001")){
				detailMap.put("HHTCNO", "WLX000001(物流周转箱)");
			}else if(detailMap.get("HHTCNO").equals("XLP000001")){
				detailMap.put("HHTCNO", "XLP000001(行李/个人物品)");
			}
			
			if(detailMap.get("HHTTYP")!=null){
				if(detailMap.get("HHTTYP").equals("RTV")||detailMap.get("HHTTYP").equals("TRF")&&detailMap.get("HHTTLC").equals("13196")){
					sumrtv = sumrtv + Double.parseDouble(detailMap.get("HHTNUM").toString());
				}else if(detailMap.get("HHTTYP").equals("TRF")&&!detailMap.get("HHTTLC").equals("13196")){
					sumtrf = sumtrf + Double.parseDouble(detailMap.get("HHTNUM").toString());
				}
			}
			
			sumvol = sumvol + Double.parseDouble(detailMap.get("HHTVOL").toString());
			sumwei = sumwei + Double.parseDouble(detailMap.get("HHTWEI").toString());
			sumnum = sumnum + Double.parseDouble(detailMap.get("HHTNUM").toString());
			
			
		}
		
		headerMap.put("sumrtv", CommonStaticUtil.doubleMath(sumrtv));
		headerMap.put("sumtrf", CommonStaticUtil.doubleMath(sumtrf));
		headerMap.put("sumvol", CommonStaticUtil.doubleMath(sumvol));
		headerMap.put("sumwei", CommonStaticUtil.doubleMath(sumwei));
		headerMap.put("sumnum", CommonStaticUtil.doubleMath(sumnum));
		
		
		return createReport(path, wb, headerMap, detailMapList, this.getTemplateName());
	}

	@Override
	public String getTemplateName() {
		// TODO Auto-generated method stub
		return PrintConfigUtil.getTemplate12Name();
	}

	@Override
	public String getPrinterName() {
		// TODO Auto-generated method stub
		return PrintConfigUtil.getTemplate11PrinterName();
	}

}
