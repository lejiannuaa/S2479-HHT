using System;
using System.Drawing;
using System.Windows.Forms;

#if DESIGNTIME
	[assembly: System.CF.Design.RuntimeAssemblyAttribute("ImageButton, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null")]
#endif

namespace HolaCore
{
    public enum Alignment
    {
        right,
        bottom
    };

    /// <summary>
    /// 显示图形的按纽
    /// </summary>
    public class ImageButton : Control
    {
        #region --- Fields ---
        private System.Drawing.Image image;
        private System.Drawing.Image imagePressed;
        private System.Drawing.Image imageDisable;
        private System.Drawing.Image m_imageBtn;
        private System.Windows.Forms.ImageList imageList;
        private int imageIndex;
        private int imagePressedIndex;
        private int imageDisableIndex;
        private int fullHeight;
        private Alignment textpos;
        private bool showframe;
        private bool isPressed;
        private bool showtext;
        private bool multiLine;
        private Bitmap m_bmpMemoire;
        private Font m_normalFont;
        private int margin;
        #endregion

        #region --- Properties ---

#if DESIGNTIME
		[System.ComponentModel.Category("配置")]
		[System.ComponentModel.DefaultValueAttribute("")]
		[System.ComponentModel.Description("定义BUTTON是否显示黑色边框")]
#endif
        public bool ShowFrame
        {
            get
            {
                return showframe;
            }
            set
            {
                showframe = value;
                this.Invalidate();
            }
        }
#if DESIGNTIME
		[System.ComponentModel.Category("配置")]
		[System.ComponentModel.DefaultValueAttribute("5")]
		[System.ComponentModel.Description("边界空白")]
#endif
        public int Margin
        {
            get
            {
                return margin;
            }
            set
            {
                margin = value;
                this.Invalidate();
            }
        }
#if DESIGNTIME
		[System.ComponentModel.Category("配置")]
		[System.ComponentModel.DefaultValueAttribute("")]
		[System.ComponentModel.Description("定义BUTTON包含的文字")]
#endif
        public bool ShowText
        {
            get
            {
                return showtext;
            }
            set
            {
                showtext = value;
                this.Invalidate();
            }
        }
#if DESIGNTIME
		[System.ComponentModel.Category("配置")]
		[System.ComponentModel.DefaultValueAttribute("")]
		[System.ComponentModel.Description("定义文字是否以多行显示")]
#endif
        public bool MultiLine
        {
            get
            {
                return multiLine;
            }
            set
            {
                multiLine = value;
                this.Invalidate();
            }
        }
#if DESIGNTIME
		[System.ComponentModel.Category("配置")]
		[System.ComponentModel.DefaultValueAttribute("")]
		[System.ComponentModel.Description("定义BUTTON包含的文字显示位置")]
#endif

        public Alignment TextPosition
        {
            get
            {
                return textpos;
            }
            set
            {
                textpos = value;
                this.Invalidate();
            }
        }
#if DESIGNTIME
		[System.ComponentModel.Category("配置")]
		[System.ComponentModel.DefaultValueAttribute("")]
		[System.ComponentModel.Description("定义BUTTON包含的文字字体")]
#endif
        public Font TextFont
        {
            get
            {
                return m_normalFont;
            }
            set
            {
                m_normalFont = value;
                this.Invalidate();
            }
        }
#if DESIGNTIME
		[System.ComponentModel.Category("配置")]
		[System.ComponentModel.DefaultValueAttribute("")]
		[System.ComponentModel.Description("用ImageList定义显示的图片")]
#endif
        public System.Windows.Forms.ImageList ButtonImageList
        {
            get
            {
                return imageList;
            }
            set
            {
                imageList = value;
                this.Invalidate();
            }
        }
#if DESIGNTIME
		[System.ComponentModel.Category("配置")]
		[System.ComponentModel.DefaultValueAttribute("")]
		[System.ComponentModel.Description("未点击时，显示的图片")]
#endif
        public System.Drawing.Image ButtonImage
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                this.Invalidate();
            }
        }
#if DESIGNTIME
		[System.ComponentModel.Category("配置")]
		[System.ComponentModel.DefaultValueAttribute("")]
		[System.ComponentModel.Description("点击时显示的图片")]
#endif
        public System.Drawing.Image ButtonPressedImage
        {
            get
            {
                return imagePressed;
            }
            set
            {
                imagePressed = value;
                this.Invalidate();
            }
        }
#if DESIGNTIME
		[System.ComponentModel.Category("配置")]
		[System.ComponentModel.DefaultValueAttribute("")]
		[System.ComponentModel.Description("ENABLE=FALSE时显示的图片")]
#endif
        public System.Drawing.Image ButtonDisableImage
        {
            get
            {
                return imageDisable;
            }
            set
            {
                imageDisable = value;
                this.Invalidate();
            }
        }

#if DESIGNTIME
		[System.ComponentModel.Category("配置")]
		[System.ComponentModel.DefaultValueAttribute("")]
		[System.ComponentModel.Description("点击时显示的图片在IMAGELIST中的序号")]
#endif
        public int ButtonImageIndex
        {
            get
            {
                return imageIndex;
            }
            set
            {
                imageIndex = value;
                if (imageList != null)
                {
                    image = imageList.Images[imageIndex];
                    this.Invalidate();
                }
            }
        }
#if DESIGNTIME
		[System.ComponentModel.Category("配置")]
		[System.ComponentModel.DefaultValueAttribute("")]
		[System.ComponentModel.Description("未点击时显示的图片在IMAGELIST中的序号")]
#endif
        public int ButtonPressedImageIndex
        {
            get
            {
                return imagePressedIndex;
            }
            set
            {
                imagePressedIndex = value;
                if (imageList != null)
                {
                    imagePressed = imageList.Images[imagePressedIndex];
                    this.Invalidate();
                }
            }
        }
#if DESIGNTIME
		[System.ComponentModel.Category("配置")]
		[System.ComponentModel.DefaultValueAttribute("")]
		[System.ComponentModel.Description("ENABLE=FALSE时显示的图片在IMAGELIST中的序号")]
#endif
        public int ButtonDisableImageIndex
        {
            get
            {
                return imageDisableIndex;
            }
            set
            {
                imageDisableIndex = value;
                if (imageList != null)
                {
                    imageDisable = imageList.Images[imageDisableIndex];
                    this.Invalidate();
                }
            }
        }


        #endregion

        public ImageButton()
            : base()
        {
            textpos = Alignment.bottom;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Brush m_btnBrush;
            Graphics gxMemoire;
            Pen m_blackPen = new System.Drawing.Pen(Color.FromArgb(0, 0, 0));
            if (m_normalFont == null)
            {
                m_normalFont = new Font("Arial", 9F, FontStyle.Regular);
            }
            if (m_bmpMemoire == null)
            {
                m_bmpMemoire = new Bitmap(ClientSize.Width, ClientSize.Height);
            }
            gxMemoire = Graphics.FromImage(m_bmpMemoire);

            gxMemoire.Clear(this.BackColor);

            m_imageBtn = new Bitmap(1, 1);
            if (isPressed)
            {
                m_btnBrush = new System.Drawing.SolidBrush(Color.FromArgb(0, 0, 255));
                if (imagePressed != null)
                {
                    m_imageBtn = imagePressed;
                }
                else if (image != null)
                {
                    m_imageBtn = image;
                }
            }
            else
            {
                m_btnBrush = new System.Drawing.SolidBrush(Color.FromArgb(0, 0, 0)); ;
                if (image != null)
                {
                    m_imageBtn = image;
                }
            }
            //如果控件设置为FALSE，则图片文字应该灰显
            if (!this.Enabled)
            {
                if (imageDisable != null)
                {
                    m_imageBtn = imageDisable;
                }
                else if (image != null)
                {
                    m_imageBtn = image;
                }
                m_btnBrush = new System.Drawing.SolidBrush(Color.Gray);
            }
            if (textpos == Alignment.right)
            {
                gxMemoire.DrawImage(m_imageBtn, margin, (ClientSize.Height - m_imageBtn.Height) / 2);
            }
            else if (textpos == Alignment.bottom)
            {
                gxMemoire.DrawImage(m_imageBtn, (ClientSize.Width - m_imageBtn.Width) / 2, margin);
            }
            //显示黑色边框
            if (showframe)
            {
                Rectangle rect;
                rect = this.ClientRectangle;
                rect.Width--;
                rect.Height--;
                gxMemoire.DrawRectangle(m_blackPen, rect);
            }
            //显示BUTTON文字内容
            if (showtext)
            {
                //增加自动换行功能
                int totalLength = ClientSize.Width - 5;    //总共可以显示文字的尺寸
                if (textpos == Alignment.right)
                {
                    totalLength = totalLength - m_imageBtn.Width - margin;;
                }
               
                SizeF siF = gxMemoire.MeasureString(this.Text, m_normalFont);	//获取设定文字的尺寸
                int textWidth = (int)siF.Width;
                if ((textWidth > totalLength) && multiLine)
                {
                    string txt1,txt2;
                    SizeF siF1,siF2;
                    int txt1Width, txt2Width;
                    //进行换行计算
                    for (int i = 0; i <= this.Text.Length; i++)
                    {
                        txt1 = this.Text.Substring(0, i + 1);
                        siF1 = gxMemoire.MeasureString(txt1, m_normalFont);	
                        txt1Width = (int)siF1.Width;
                        if (txt1Width > totalLength)
                        {
                            txt1 = this.Text.Substring(0, i);
                            siF1 = gxMemoire.MeasureString(txt1, m_normalFont);	
                            if (textpos == Alignment.right)
                            {
                                gxMemoire.DrawString(txt1, m_normalFont, m_btnBrush, margin + m_imageBtn.Width + 5, ClientSize.Height / 2 - (int)siF1.Height);
                            }
                            else if (textpos == Alignment.bottom)
                            {
                                gxMemoire.DrawString(txt1, m_normalFont, m_btnBrush, (ClientSize.Width - siF1.Width) / 2, margin + m_imageBtn.Height + 5);
                            }
                            for (int k = 0; k <= this.Text.Length - i; k++)
                            {

                                txt2 = this.Text.Substring(i, k);
                                siF2 = gxMemoire.MeasureString(txt2, m_normalFont);
                                txt2Width = (int)siF2.Width;
                                if (txt2Width > totalLength)
                                {
                                    txt2 = this.Text.Substring(i, k - 1);
                                    siF2 = gxMemoire.MeasureString(txt2, m_normalFont);
                                    if (textpos == Alignment.right)
                                    {
                                        gxMemoire.DrawString(txt2, m_normalFont, m_btnBrush, margin + m_imageBtn.Width + 5, ClientSize.Height / 2 + 2);
                                    }
                                    else if (textpos == Alignment.bottom)
                                    {
                                        gxMemoire.DrawString(txt2, m_normalFont, m_btnBrush, (ClientSize.Width - siF2.Width) / 2, margin + m_imageBtn.Height + 5 + (int)siF2.Height);
                                    }
                                    break;
                                }
                                else if (k == this.Text.Length - i)
                                {
                                    txt2 = this.Text.Substring(i);
                                    siF2 = gxMemoire.MeasureString(txt2, m_normalFont);
                                    if (textpos == Alignment.right)
                                    {
                                        gxMemoire.DrawString(txt2, m_normalFont, m_btnBrush, (ClientSize.Width - siF2.Width + margin + m_imageBtn.Width + 5) / 2, ClientSize.Height / 2 + 2);

                                    }
                                    else if (textpos == Alignment.bottom)
                                    {
                                        gxMemoire.DrawString(txt2, m_normalFont, m_btnBrush, (ClientSize.Width - siF2.Width) / 2, margin + m_imageBtn.Height + 5 + (int)siF2.Height);
                                    }
                                    break;
                                }

                            }
                            break;
                        }
                    }

                }
                else
                {
                    //计算文字宽度
                    SizeF textsize = gxMemoire.MeasureString(this.Text, m_normalFont);
                    
                    if (textpos == Alignment.right)
                    {
                        gxMemoire.DrawString(this.Text, m_normalFont,m_btnBrush,margin + m_imageBtn.Width + 5,(ClientSize.Height - textsize.Height) / 2);
                    }
                    else if (textpos == Alignment.bottom)
                    {	
                        gxMemoire.DrawString(this.Text, m_normalFont, m_btnBrush,(ClientSize.Width - textsize.Width) / 2, margin + m_imageBtn.Height + 5);
                    }
                }
            }
            e.Graphics.DrawImage(m_bmpMemoire, 0, 0);

            gxMemoire.Dispose();

            base.OnPaint(e);
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);
            isPressed = true;
            this.Invalidate();
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            isPressed = false;
            this.Invalidate();
        }

        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            if (m_bmpMemoire != null)
            {
                m_bmpMemoire.Dispose();
                m_bmpMemoire = null;
            }
            this.Invalidate();
        }

        protected override void OnEnabledChanged( System.EventArgs e)
        {
            base.OnEnabledChanged(e);
            this.Invalidate();
        }

        protected override void OnTextChanged(System.EventArgs e)
        {
            base.OnTextChanged(e);
            this.Invalidate();
        }
        /// <summary>
        ///  实际图形占用的尺寸
        /// </summary>
        /// <returns></returns>
        public int getFullHeight()
        {
            int totalLength = ClientSize.Width - 5;   
            if (textpos == Alignment.right)
            {
                totalLength = totalLength - image.Width - margin; ;
            }

            Graphics g = this.CreateGraphics();
            SizeF textsize = g.MeasureString(this.Text, m_normalFont);
            int textWidth = (int)textsize.Width;
            if ((textWidth > totalLength) && multiLine)
            {
                if (textpos == Alignment.right)
                {
                    fullHeight = (int)textsize.Height * 2 + 2;
                    if (image.Height > fullHeight)
                    {
                        fullHeight = image.Height;
                    }
                }
                else if (textpos == Alignment.bottom)
                {
                    fullHeight = (int)(int)textsize.Height * 2 + 2 + image.Height + 5; 
                }
            }
            else
            {
                if (textpos == Alignment.right)
                {
                    fullHeight = (int)textsize.Height;
                    if (image.Height > fullHeight)
                    {
                        fullHeight = image.Height;
                    }
                }
                else if (textpos == Alignment.bottom)
                {
                    fullHeight = (int)textsize.Height + image.Height + 5;
                }
            }
            return fullHeight;
        }
    }
}
