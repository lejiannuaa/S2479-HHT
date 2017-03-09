package com.hola.bs.service.hht;

import java.io.File;
import java.io.IOException;
import java.util.Date;

import javax.print.PrintException;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.apache.log4j.MDC;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.DateUtils;
import com.hola.bs.util.FTPUtil;
import com.hola.bs.util.PrintUtil;

/**
 * 
 * 实现 收货作业--PO收货--PO照相
 * 
 * @author 
 * @version 
 */
public class HHT_006_01 extends BusinessService implements ProcessUnit {

	public Log log = LogFactory.getLog(getClass());
	

	public String process(Request request) {
		BusinessBean bean = new BusinessBean();
		try {
			bean = resolveRequest(request);
			processData(bean);
		} catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。" + e.getMessage());
			log.error("", e);
		}
		// log.info("response=" + bean.getResponse().toString());
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());

		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception {
		String store = "13101";
		if (bean.getUser() != null) {
			store = bean.getUser().getStore();
		}
		String usr=bean.getUser().getName();
		String strPhoto = (String) bean.getRequest().getParameter("photo");
		String[] photos = strPhoto.split(",");
		log.info("recieve photostr = "+strPhoto);
		FTPUtil ftp = new FTPUtil(
				configpropertyUtil.getValue("imgprint.ftp.host"));
		try {
			log.info("ftp connect to "+configpropertyUtil.getValue("imgprint.ftp.host"));
			ftp.connect(configpropertyUtil.getValue("imgprint.ftp.user"),
					configpropertyUtil.getValue("imgprint.ftp.password"));
			log.info("ftp connect ...ok.");
			ftp.binary();

			String localPath = configpropertyUtil.getValue("imgprint.localpath")+System.getProperty("file.separator")
			+"PO收货"+System.getProperty("file.separator")+DateUtils.date2String2(new Date());
			File path=new File(localPath);
			if (!path.exists()){
				path.mkdirs();
			}
			PrintUtil dp = new PrintUtil();
			String printDevice = configpropertyUtil.getValue("imgprint.device");

			for (int i = 0; i < photos.length; i++) {
				if (photos[i] != null && !"".equals(photos[i].trim())) {
					String imgFileName = photos[i];
					File localImgFile = new File(localPath
							+ System.getProperty("file.separator")
							+ imgFileName);
					log.info("download file from: "+store + "/" + imgFileName+" to : "+localImgFile.getAbsolutePath());
					ftp.downloadFile(localImgFile, store + "/" + imgFileName);
//					log.info("print image "+localImgFile.getAbsolutePath());
//					dp.printImage(localImgFile.getAbsolutePath(), printDevice);
//					localImgFile.delete();
				}
			}
			bean.getResponse().setCode(BusinessService.successcode);
		}finally {
			ftp.disconnect();
		}
		
	}
}
