package com.hola.bs.service;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStreamWriter;
import java.sql.SQLException;
import java.text.DecimalFormat;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.context.ApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;
import org.springframework.context.support.FileSystemXmlApplicationContext;
import org.springframework.stereotype.Service;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.JdbcTemplateSqlServerUtil;
import com.hola.bs.util.Config;
import com.hola.bs.util.Root;
import com.hola.bs.util.XmlElement;
import com.hola.bs.util.XmlUtil;


@Service
public class FinancialService {
	@Autowired(required=true)
	@Qualifier("jdbcTemplateSqlServerUtil")
	protected JdbcTemplateSqlServerUtil jdbcTemplateSqlServerUtil;
	
	public JdbcTemplateSqlServerUtil getJdbcTemplateSqlServerUtil() {
		return jdbcTemplateSqlServerUtil;
	}

	public void setJdbcTemplateSqlServerUtil(
			JdbcTemplateSqlServerUtil jdbcTemplateSqlServerUtil) {
		this.jdbcTemplateSqlServerUtil = jdbcTemplateSqlServerUtil;
	}
	
	public static Double doubleMath(Double allPrice) {
		DecimalFormat df = new DecimalFormat("##.00");
		String format = df.format(allPrice);
		return Double.valueOf(format);
	}
	
	 public static void main(String[] args) throws Exception {
		 
		    ApplicationContext ctx = new ClassPathXmlApplicationContext("spring.xml");
			
			JdbcTemplateSqlServerUtil t=(JdbcTemplateSqlServerUtil)ctx.getBean("jdbcTemplateSqlServerUtil");
			File file = new File("D:\\10月换开明细.xlsx");
			List<Object[]> f = XmlUtil.getObjectByExcel(file);
			HashMap<String,String> map = new HashMap<String, String>();
			System.out.println("f="+f.size());
			for(Object[] o : f){
				System.out.println("1="+o[1]+"   4="+o[4]);
				if(o[1]!=null){
					map.put(o[1].toString(), ((o[4] != null) ? String.valueOf(doubleMath((Double)o[4])) : "0.00"));
				}
				
			}

			List<Map> l=t.searchForList("SELECT sys_storeno + CONVERT (VARCHAR, txn_date, 112) + ecr_no + CONVERT (VARCHAR, txn_no) + txn_guibegno WYM, NULL FPDM, NULL FPHM, NULL YFPDM, NULL YFPHM, CONVERT (VARCHAR, txn_date, 112) KPRQ,  NULL FKFDM, NULL FKFMC, CASE WHEN TXN_STATUS='N' THEN '1' WHEN TXN_STATUS='C' THEN '3' WHEN TXN_STATUS='D' THEN '3' END FKTKZT, TXN_GUICnt DBFPSL , abs(TXN_TotQty) SPHSL , abs(TXN_TotSaleAmt) SPHJJE , abs(TXN_TotGui) SJKPJE   FROM pos3008 WHERE txn_date >= '2015-10-01' and txn_date <= '2015-10-31'");
			
			for(Map tmap : l){
				if(map.get(tmap.get("WYM").toString().substring(tmap.get("WYM").toString().length()-8))!=null){
					
					tmap.put("FKTKZT", "3");
					tmap.put("DBFPSL", "1");
					tmap.put("SPHJJE", map.get(tmap.get("WYM").toString().substring(tmap.get("WYM").toString().length()-8)).toString());
					tmap.put("SJKPJE", map.get(tmap.get("WYM").toString().substring(tmap.get("WYM").toString().length()-8)).toString());
				}

			}



			
			
			XmlElement[] xml = new XmlElement[l.size()];
			xml[0] = new XmlElement("FPHZXX", l);
			writerFile(xml);
			
			System.out.println(l.size());
	 
	 
	 }
	
	

	
	
	
	public static void writerFile(XmlElement xml[])
			throws Exception {

		String path = "D:\\";
			
		System.out.println("文件输出:" + path);
		File f = new File(path);

		if (!f.exists())
			CreateDirs(path);

		path += "TAX_FPXX_20151001_330227793032992_1.xml";

		FileOutputStream fos = null;
		BufferedWriter out = null;

		String s = toXML(xml);

		try {
			fos = new FileOutputStream(path);
			out = new BufferedWriter(new OutputStreamWriter(fos, "UTF-8"));
			out.write(s);
		} finally {
			if (out != null)
				out.close();
			if (fos != null)
				fos.close();
		}

		// fw = new FileWriter(path);
		// fw.write(s, 0, s.length());
		// fw.flush();

	}
	
	public static void CreateDirs(String path) {
		File f = new File(path);
		if (!f.exists())
			f.mkdirs();

	}
	
	public static String toXML(XmlElement xml[]) throws SQLException,IOException {
		
		Root root = new Root();

		if (xml != null) {
			for (XmlElement x : xml) {
				root.addXmlElement(x);
			}
		}
		return root.asXmlFin();
	}


}
