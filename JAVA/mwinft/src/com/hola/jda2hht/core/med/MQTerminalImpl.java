package com.hola.jda2hht.core.med;

import java.io.File;
import java.util.List;

import org.apache.log4j.Logger;

import com.hola.jda2hht.model.ChangeInfoBean;
import com.hola.jda2hht.model.CsvBean;
import com.hola.jda2hht.util.ConfigUtil;
import com.hola.jda2hht.util.JMSSender;
import com.hola.jda2hht.util.ZipUtils;

/**
 * MQ终端
 * 
 * @author 唐植超(上海软通)
 * @date 2013-1-7
 */
public class MQTerminalImpl implements ITerminal {

	private static final Logger log = Logger.getLogger(MQTerminalImpl.class);

	@Override
	public void upload(ChangeInfoBean infoBean, List<CsvBean> dbList)
			throws Exception {
		log.info("连接MQ");
		JMSSender send = ConfigUtil.createMQ(infoBean);
		for (CsvBean cb : dbList) {
			try {
				log.info("开始上传MQ" + cb.getFilePath());
				File csvFile = new File(cb.getFilePath());
				// zip文件的文件名和csv的文件名称一样
				String path = csvFile.getAbsolutePath();
				int index = path.lastIndexOf(".");
				path = path.substring(0, index);
				File zipFile = new File(path + ".zip");
				// 压缩
				ZipUtils.zipFile(csvFile, zipFile);
				// 上传mq
				send.sendBinaryMsg(zipFile, infoBean.getOqname());
				log.info("上传MQ" + cb.getFilePath() + "结束");
			} catch (Exception e) {
				e.printStackTrace();
				log.error("MQ上传报错：" + e.getMessage(), e);
			}
		}
		log.info("关闭连接");
	}

}
