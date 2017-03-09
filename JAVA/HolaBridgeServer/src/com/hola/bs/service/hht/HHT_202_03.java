package com.hola.bs.service.hht;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.remoting.RemoteLookupFailureException;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.print.rmi.PrintServer;
import com.hola.bs.print.template.PrintTemplate8Rtv;
import com.hola.bs.print.template.PrintTemplate8Trf;
import com.hola.bs.service.BusinessService;
/**
 * 出货箱单 头档
 * 
 * @author S2138 HHT_202_04
 * 2013 Mar 18, 2013 3:53:18 PM
 */
public class HHT_202_03 extends BusinessService implements ProcessUnit{
	@Autowired(required = true)
	private PrintTemplate8Rtv printTemplate8Rtv;
	
	@Autowired(required = true)
	private PrintTemplate8Trf printTemplate8Trf;

	private PrintServer printServer;
	
	public String process(Request request) {
		BusinessBean bean = new BusinessBean();
		try {
			bean = resolveRequest(request);
			printServer = super.createPrintClient(bean.getUser().getStore());
			//查询单据下所有的箱号
			String bc = request.getParameter("bc");
			List<String> list = getHHTCNO(bean.getUser().getStore(), bc);
			String outType = request.getParameter("type");//获取出货类型
			if(outType.equalsIgnoreCase("RTV")){//退货单
				for(String hhtcno : list){
					String fileMettle = printTemplate8Rtv.createReport(
						bean.getUser().getOwnerFilePath(), 
						bean.getUser().getStore(),
						new String[]{bean.getUser().getStore(),hhtcno,outType});
					printServer.print(bean.getUser().getName()+"\\"+fileMettle);
					Thread.sleep(1000);
				}
			}else if(outType.equalsIgnoreCase("TRF")){//调拨单
				for(String hhtcno : list){
					String fileMettle = printTemplate8Trf.createReport(
						bean.getUser().getOwnerFilePath(), 
						bean.getUser().getStore(),
						new String[]{bean.getUser().getStore(),hhtcno,outType});
					printServer.print(bean.getUser().getName()+"\\"+fileMettle);
					Thread.sleep(1000);
				}
			}
			
		}catch(RemoteLookupFailureException rlfe){
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("调用远程服务失败，请检查店铺"+ bean.getUser().getStore()+"的网络线路状况以及打印服务是否正常启动。");
		}catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。" + e);
			log.error("", e);
		}
		return bean.getResponse().toString();
	}
	
	private List<String> getHHTCNO(String store, String hhtno){
		
		List map = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(store,
		"hht202.03.01"), new Object[] {hhtno});
		
		List<String> list = new ArrayList<String>();
		for(int i=0;i<map.size();i++){
			if(map.get(i)!=null)
				list.add(((Map)map.get(i)).get("HHTCNO").toString());
		}
		
		return list;
	}

	public PrintTemplate8Rtv getPrintTemplate8Rtv() {
		return printTemplate8Rtv;
	}

	public void setPrintTemplate8Rtv(PrintTemplate8Rtv printTemplate8Rtv) {
		this.printTemplate8Rtv = printTemplate8Rtv;
	}

	public PrintTemplate8Trf getPrintTemplate8Trf() {
		return printTemplate8Trf;
	}

	public void setPrintTemplate8Trf(PrintTemplate8Trf printTemplate8Trf) {
		this.printTemplate8Trf = printTemplate8Trf;
	}

	public PrintServer getPrintServer() {
		return printServer;
	}

	public void setPrintServer(PrintServer printServer) {
		this.printServer = printServer;
	}

	
}
