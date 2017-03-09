package com.hola.common.csv.model;

import java.io.BufferedReader;
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
public class DataBean implements Serializable {

	private static final long serialVersionUID = 1L;
	// 文件路地址
	private String filePath;
	// 一个层
	private List<LayerBean> layerList = new LinkedList<LayerBean>();

	public DataBean() {
	}

	/**
	 * 给一个固定的字符串给我，我会按照csv的规则解析
	 * 
	 * @param data
	 * @param separator
	 * @param ln
	 */
	public DataBean(String data, String separator, String ln) {
		getCsv(data, separator, ln);
	}

	/**
	 * 支持一行一行的读取，并解析，但是该类不负责关闭文件句柄，不处理乱码问题
	 * 
	 * @param reader
	 * @param separator 分隔符
	 * @param ln 换行符
	 * @throws Exception
	 */
	public DataBean(BufferedReader reader, String separator, String ln) throws Exception
	{
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
	@SuppressWarnings("null")
	private void getCsv(BufferedReader reader, String separator, String ln)
			throws Exception 
	{
		
		
		String line;
		StringBuilder sb = new StringBuilder();
		// 一行一行的读取
		boolean haveEND = false; 
		while ((line = reader.readLine()) != null) 
		{
			// 没读取一行，都添加到sb中，并追加换行符
			line = new String(line.getBytes(), "utf-8");
			if(line.toUpperCase().indexOf("END") > -1)
				haveEND = true;
			sb.append(line);
			sb.append(ln);
			// 没一层数据读取结束，就进行一次解析
			if (line.startsWith(LayerBean.EOF + separator)) {
				String[] data = sb.toString().split("\n");
				this.layerList.add(new LayerBean(data, separator));
				sb = new StringBuilder();
			}
		}
		if(haveEND == false)
		{
			Object o = null;
			o.hashCode();
		}
	}
	public List<LayerBean> getLayerList() {
		return layerList;
	}

	public void setLayerList(List<LayerBean> layerList) {
		this.layerList = layerList;
	}

	public String getFilePath() {
		return filePath;
	}

	public void setFilePath(String filePath) {
		this.filePath = filePath;
	}

	/**
	 * 转成csv格式
	 * 
	 * @file: DataBean.java
	 * @author 唐植超(上海软通)
	 * @date 2012-12-28
	 * @param separator
	 * @param ln
	 * @return
	 */
	public String toCsv(String separator, String ln) {
		StringBuilder sb = new StringBuilder();
		for (LayerBean lb : layerList) {
			sb.append(lb.toCsv(separator, ln));
		}
		sb.append("END");
		return sb.toString();
	}

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
}
