package com.hola.bs.service.hht;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.remoting.RemoteLookupFailureException;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.print.PrintConfigUtil;
import com.hola.bs.print.rmi.PrintServer;
import com.hola.bs.print.template.PrintTemplate5;
import com.hola.bs.print.template.PrintTemplate7;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.CommonStaticUtil;

public class HHT_202_04 extends BusinessService implements ProcessUnit{
	
	@Autowired(required = true)
	private PrintTemplate5 printTemplate5;//po出货单
	
	@Autowired(required = true)
	private PrintTemplate7 printTemplate7;//调拨出货单
	
//	@Autowired(required = true)
//	private PrintTemplate8Rtv printTemplate8Rtv;
//	
//	@Autowired(required = true)
//	private PrintTemplate8Trf printTemplate8Trf;

	private PrintServer printServer;
	
	public String process(Request request) {
		BusinessBean bean = new BusinessBean();
		try {
			bean = resolveRequest(request);
			printServer = super.createPrintClient(bean.getUser().getStore());
			//查询单据下所有的箱号
			String bc = request.getParameter("bc");
			List<String[]> list = getHHTCNO(bean.getUser().getStore(), bc);
			String outType = request.getParameter("type");//获取出货类型
			if(outType.equalsIgnoreCase("RTV")){//退货单
				for(String[] s : list){
					String file = printTemplate5.createReport(
							bean.getUser().getOwnerFilePath(), 
							bean.getUser().getStore(),
							new String[]{bc, s[0],s[1]},
							bc, s[0]);
//					System.out.println("=========="+bean.getUser().getName()+"\\"+file);
//					String mattleFile = 
					printServer.print(bean.getUser().getName()+"\\"+file);
					Thread.sleep(1000);//这里会出现打印顺序混乱，加sleep控制一下
					
//					String fileMettle = printTemplate8Rtv.createReport(
//						bean.getUser().getOwnerFilePath(), 
//						bean.getUser().getStore(),
//						new String[]{bean.getUser().getStore(),hhtcno,outType});
//					printServer.print(bean.getUser().getName()+"\\"+fileMettle);
//					Thread.sleep(1000);
				}
				
//				String[] files = printTemplate8Rtv.createRtvReport(
//						bean.getUser().getOwnerFilePath(), 
//						bean.getUser().getStore(),
//						new String[]{bean.getUser().getStore(),bc,outType});
////				System.out.println("RTV");
////				System.out.println(Arrays.toString(files));
//				if(files!=null&&files.length>0){
//					for(String f : files){
//						printServer.print(bean.getUser().getName()+"\\"+f);
////						this.wait(2000);
//					}
//				}

			}else if(outType.equalsIgnoreCase("TRF")){//调拨单
				for(String[] hhtcno : list){
					String file = printTemplate7.createReport(
							bean.getUser().getOwnerFilePath(), 
							bean.getUser().getStore(),
							new String[]{bc, hhtcno[0]},
							hhtcno[0]);
//					System.out.println("=========="+bean.getUser().getName()+"\\"+file);
					printServer.print(bean.getUser().getName()+"\\"+file);
					Thread.sleep(1000);
					
//					String fileMettle = printTemplate8Trf.createReport(
//						bean.getUser().getOwnerFilePath(), 
//						bean.getUser().getStore(),
//						new String[]{bean.getUser().getStore(),hhtcno,outType});
//					printServer.print(bean.getUser().getName()+"\\"+fileMettle);
//					Thread.sleep(1000);
				}
//				String[] trfFiles = printTemplate8Trf.createTrfReport(
//						bean.getUser().getOwnerFilePath(), 
//						bean.getUser().getStore(),
//						new String[]{bean.getUser().getStore(),bc,outType});
////				System.out.println("TRF");
////				System.out.println(Arrays.toString(files));
//				if(trfFiles!=null&&trfFiles.length>0){
//					for(String f : trfFiles){
//						printServer.print(bean.getUser().getName()+"\\"+f);
//					}
//				}
				
			}
			
		}catch(RemoteLookupFailureException rlfe){
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("调用远程服务失败，请检查店铺"+ bean.getUser().getStore()+"的网络线路状况以及打印服务是否正常启动。");
//			log.error("", rlfe);
		}catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。" + e);
			log.error("", e);
		}
//		log.info("response=" + bean.getResponse().toString());
//		MDC.put("userNo", bean.getUser().getName());
//		MDC.put("stoNo", bean.getUser().getStore());
//		log.info("列印出货单操作, response="+bean.getResponse().toString());
		return bean.getResponse().toString();
	}
	
	private List<String[]> getHHTCNO(String store, String hhtno){
		
		List map = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(store,
		"hht202.03.02"), new Object[] {hhtno});
		
		List<String[]> list = new ArrayList<String[]>();
		for(int i=0;i<map.size();i++){
			if(map.get(i)!=null){
				
				String[] s = new String[]{((Map)map.get(i)).get("HHTCNO").toString(),CommonStaticUtil.strIsNull(((Map)map.get(i)).get("HHTVTP"))};
				list.add(s);
			}
				
		}
		
		return list;
	}

	public PrintTemplate5 getPrintTemplate5() {
		return printTemplate5;
	}

	public void setPrintTemplate5(PrintTemplate5 printTemplate5) {
		this.printTemplate5 = printTemplate5;
	}

	public PrintTemplate7 getPrintTemplate7() {
		return printTemplate7;
	}

	public void setPrintTemplate7(PrintTemplate7 printTemplate7) {
		this.printTemplate7 = printTemplate7;
	}

	public PrintServer getPrintServer() {
		return printServer;
	}

	public void setPrintServer(PrintServer printServer) {
		this.printServer = printServer;
	}
	

	
}
