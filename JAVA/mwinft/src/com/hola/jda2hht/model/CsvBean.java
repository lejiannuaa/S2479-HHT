package com.hola.jda2hht.model;

import java.io.BufferedReader;
import java.io.File;
import java.io.RandomAccessFile;
import java.io.Serializable;
import java.util.LinkedList;
import java.util.List;

/**
 * 数据库查询出来的数据封装的对象
 * 
 * 这个对象可能会被输出成csv，txt，db等多种格式
 * 
 * @remark
 * @author 唐植超(上海软通)
 * @date 2012-12-26
 */
public class CsvBean implements Serializable {

	private static final long serialVersionUID = 1L;
	// 一个层
	private String filePath;

	public String getFilePath() {
		return filePath;
	}

	public void setFilePath(String filePath) {
		this.filePath = filePath;
	}

	private List<LayerBean> layerList = new LinkedList<LayerBean>();
	private String separator;
	private String ln;
	private RandomAccessFile operFile;

	public CsvBean(String separator, String ln, File file) throws Exception {
		this.separator = separator;
		this.ln = ln;
		file.createNewFile();
		this.filePath = file.getAbsolutePath();
		operFile = new RandomAccessFile(file, "rws");
	}

	/**
	 * 给一个固定的字符串给我，我会按照csv的规则解析
	 * 
	 * @param data
	 * @param separator
	 * @param ln
	 */
	public CsvBean(String data, String separator, String ln) {
		getCsv(data, separator, ln);
	}

	/**
	 * 支持一行一行的读取，并解析，但是该类不负责关闭文件句柄，不处理乱码问题
	 * 
	 * @param reader
	 * @param separator
	 * @param ln
	 * @throws Exception
	 */
	public CsvBean(BufferedReader reader, String separator, String ln)
			throws Exception {
		getCsv(reader, separator, ln);
	}

	/**
	 * 通过读取文件流解析的过程
	 * 
	 * @file: DataBean.java
	 * @author 唐植超(上海软通)
	 * @date 2012-12-28
	 * @param reader
	 * @param separator
	 * @param ln
	 * @throws Exception
	 */
	private void getCsv(BufferedReader reader, String separator, String ln)
			throws Exception {
		String line;
		StringBuilder sb = new StringBuilder();
		// 一行一行的读取
		while ((line = reader.readLine()) != null) {
			// 每读取一行，都添加到sb中，并追加换行符
			sb.append(line);
			sb.append(ln);
			// 每一层数据读取结束，就进行一次解析
			if (line.startsWith(LayerBean.EOF + separator)) {
				String[] data = sb.toString().split("\n");
				this.layerList.add(new LayerBean(data, separator));
				sb = new StringBuilder();
			}
		}
	}

	public List<LayerBean> getLayerList() {
		return layerList;
	}

	public void setLayerList(List<LayerBean> layerList) {
		this.layerList = layerList;
	}

	// /**
	// * 转成csv格式
	// *
	// * @file: DataBean.java
	// * @author 唐植超(上海软通)
	// * @date 2012-12-28
	// * @return
	// */
	// public String toCsv() {
	// StringBuilder sb = new StringBuilder();
	// for (LayerBean lb : layerList) {
	// sb.append(lb.toCsv(separator, ln));
	// }
	// sb.append("END");
	// return sb.toString();
	// }

	/**
	 * 解析过程
	 * 
	 * @file: DataBean.java
	 * @author 唐植超(上海软通)
	 * @date 2012-12-28
	 * @param data
	 * @param separator
	 * @param ln
	 */
	private void getCsv(String data, String separator, String ln) {
		String prefix = LayerBean.BOF + separator;
		// 按照 bof\t分隔出来的一定是一层一层的
		String[] dataArray = data.split(prefix);
		if (dataArray == null || dataArray.length == 0) {
			return;
		}
		for (int i = 0; i < dataArray.length; i++) {
			if (dataArray[i] == null || dataArray[i].trim().equals("")) {
				continue;
			}
			// 循环解析数据 ，一定要把前缀加回去不然，数据就不完整了
			String[] layData = (prefix + dataArray[i]).split(ln);
			this.layerList.add(new LayerBean(layData, separator));
		}
	}

	/**
	 * layer的头
	 * 
	 * @file: CsvBean.java
	 * @author 唐植超(上海软通)
	 * @date 2012-12-31
	 * @param layer
	 */
	public void writeLayHead(LayerBean layer) throws Exception {
		StringBuilder sb = new StringBuilder();
		sb.append(LayerBean.BOF);
		sb.append(this.separator);
		sb.append(layer.getSysName());// 来源系统
		sb.append(this.separator);
		sb.append(layer.getMsgCode());// 电文代码
		sb.append(this.separator);
		sb.append(layer.getLayCode());// 层代码
		sb.append(this.separator);
		sb.append(layer.getChgname());
		sb.append(ln);
		this.operFile.write(sb.toString().getBytes("utf-8"));
	}

	/**
	 * 中间的数据
	 * 
	 * @file: CsvBean.java
	 * @author 唐植超(上海软通)
	 * @date 2012-12-31
	 * @param dataList
	 * @throws Exception
	 */
	public void appendLayData(List<String[]> dataList) throws Exception {
		StringBuilder sb = new StringBuilder();
		for (String[] data : dataList) {
			// 添加一条数据
			sb.append(LayerBean.DTA);
			sb.append(separator);
			String line = "";
			for (int i = 0; i < data.length; i++) {
				// 写一列
				line += data[i];
				if (i < data.length - 1) {
					line += separator;
				}
			}
			sb.append(line);
			// 一行数据写完，换行
			sb.append(ln);
		}
		this.operFile.write(sb.toString().getBytes("utf-8"));
	}

	/**
	 * layer的尾
	 * 
	 * @file: CsvBean.java
	 * @author 唐植超(上海软通)
	 * @date 2012-12-31
	 * @param layer
	 * @throws Exception
	 */
	public void wrirteLayFoot(LayerBean layer) throws Exception {
		StringBuilder sb = new StringBuilder();
		// 结束行
		sb.append(LayerBean.EOF);
		sb.append(this.separator);
		sb.append(layer.getSysName());// 来源系统
		sb.append(this.separator);
		sb.append(layer.getMsgCode());// 电文代码
		sb.append(this.separator);
		sb.append(layer.getLayCode());// 层代码
		sb.append(this.separator);
		sb.append(layer.getSize());// 数量
		// 加个换行符
		sb.append(ln);
		this.operFile.write(sb.toString().getBytes("utf-8"));
	}

	/**
	 * 写入结尾
	 * 
	 * @file: CsvBean.java
	 * @author 唐植超(上海软通)
	 * @throws Exception
	 * @date 2012-12-31
	 */
	public void writeEnd() throws Exception {
		this.operFile.write("END".getBytes("utf-8"));
	}

}
