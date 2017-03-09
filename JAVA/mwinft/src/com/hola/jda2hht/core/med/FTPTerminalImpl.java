package com.hola.jda2hht.core.med;

import java.io.File;
import java.util.List;

import org.apache.log4j.Logger;

import com.hola.jda2hht.model.ChangeInfoBean;
import com.hola.jda2hht.model.CsvBean;
import com.ibm.as400.access.FTP;

public class FTPTerminalImpl implements ITerminal {
	private static final Logger log = Logger.getLogger(FTPTerminalImpl.class);

	@Override
	public void upload(ChangeInfoBean infoBean, List<CsvBean> dbList)
			throws Exception {
		try {
			String host = "";
			String dir = "";
			String url = infoBean.getFtpurl().substring(6);
			int index = url.indexOf("/");
			host = url.substring(0, index);
			dir = url.substring(index);
			// 分割地址和目录
			log.info("执行FTP上传，地址：" + infoBean.getFtpurl());
			FTP ftp = new FTP(host, infoBean.getFtpusername(),
					infoBean.getFtppwd());
			ftp.connect();
			String[] dirArray = dir.split("/");
			for (String str : dirArray) {
				if (null == str || str.trim().equals("")) {
					continue;
				}
				ftp.cd(str);
			}
			System.out.println("ftp目录：" + ftp.getCurrentDirectory());
			for (CsvBean db : dbList) {
				log.info(" 文件地址：" + db.getFilePath());
				File f = new File(db.getFilePath());
				try {
					log.info("开始上传...");
					ftp.put(f, f.getName());
					log.info("上传完毕...");
				} catch (Exception e) {
					log.error("上传文件失败：" + db.getFilePath() + " "
							+ e.getMessage() , e);
					throw e;
				}
			}
			ftp.disconnect();
			log.info("ftp上传完毕");
		} catch (Exception e) {
			log.error("ftp文件上传出现错误！" + e.getMessage(),e);
			throw e;
		} finally {
			try {
				// 关闭连接
				// client.logout();
				// client.close();
			} catch (Exception e2) {
				throw e2;
			}
		}
	}
}
