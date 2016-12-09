using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smobiler.Core;
using Smobiler.Core.Controls;
using SmoONE.Domain;
using SmoONE.CommLib;
using SmoONE.DTOs;

namespace SmoONE.UI.CostCenter
{
    // ******************************************************************
    // �ļ��汾�� SmoONE 1.0
    // Copyright  (c)  2016-2017 Smobiler
    // ����ʱ�䣺 2016/11
    // ��Ҫ���ݣ�  �ɱ�����ģ�崴����༭����
    // ******************************************************************
    partial class frmCostTempletCreate : Smobiler.Core.MobileForm
    {
        #region "definition"
        public string CTempID;//ģ����
        string type = "";//����
        private int AEACheckTop ;//����������top
        private int imgCheckLeft = 0;
        private string addAEACheck = "";
        private List<string> listAEAChecks = new List<string>(); //����������
        private List<ImageButton> listbtnAEAChecksP = new List<ImageButton>();//����������ͷ��ؼ�
        private List<Button> listbtnAEAChecks = new List<Button>();//�������������ƿؼ�

        private int FCheckTop;//����������top
        private int imgFCheckLeft = 0;
        private string addFCheck = "";
        private List<string> listFCheckers = new List<string>(); //����������
        private List<ImageButton> listbtnFCheckersP = new List<ImageButton>();//����������ͷ��ؼ�
        private List<Button> listbtnFCheckers = new List<Button>();//�������������ƿؼ�
        AutofacConfig AutofacConfig = new AutofacConfig();//����������
        #endregion
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btntype_Click(object sender, EventArgs e)
        {
            popType.Groups.Clear();
            PopListGroup poli = new PopListGroup();
            popType.Groups.Add(poli);
            poli.Text = "����ѡ��";
            //��ȡ���ͣ�����ֵpoplist����
            List<CostCenter_Type> listCCType = AutofacConfig.costCenterService.GetAllCCType();
            foreach (CostCenter_Type ccType in listCCType)
            {
                poli.Items.Add(ccType.CC_T_Description, ccType.CC_T_TypeID);
                if (type.Trim().Length > 0)
                {
                    if (type.Trim().Equals(ccType.CC_T_TypeID))
                    {
                        popType.SetSelections(poli.Items[(poli.Items.Count - 1)]);
                    }
                }
            }
            popType.ShowDialog();
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(type) == true)
                {
                    throw new Exception("���������ͣ�");
                }
                if (listAEAChecks.Count <=0)
                {
                    throw new Exception("���������������ˣ�");
                }
                if (listFCheckers.Count <= 0)
                {
                    throw new Exception("��������������ˣ�");
                }
              
                CCTTInputDto ccTemplate = new CCTTInputDto();
                ccTemplate.CC_TT_TypeID = type;
                //����������
                string AEAChecks = "";
                foreach (string checkuser in listAEAChecks)
                {
                    if (string.IsNullOrWhiteSpace(AEAChecks) == true)
                    {
                        AEAChecks = checkuser;
                    }
                    else
                    {
                        AEAChecks += "," + checkuser;
                    }
                }
                ccTemplate.CC_TT_AEACheckers = AEAChecks;
                //����������
                string FCheckers = "";
                foreach (string checkuser in listFCheckers)
                {
                    if (string.IsNullOrWhiteSpace(FCheckers) == true)
                    {
                        FCheckers = checkuser;
                    }
                    else
                    {
                        FCheckers += "," + checkuser;
                    }
                }
                ccTemplate.CC_TT_FinancialCheckers = FCheckers;
                ccTemplate.CC_TT_UpdateUser = Client.Session["U_ID"].ToString();
               ReturnInfo result;
                if (string.IsNullOrEmpty(CTempID) ==false  )
                {
                    ccTemplate.CC_TT_TemplateID = CTempID;
                    //���³ɱ�����ģ����Ϣ
                    result = AutofacConfig.costCenterService.UpdateCC_Type_Template (ccTemplate);
                }
                else 
                {
                   //�����ɱ�����ģ��
                    result = AutofacConfig.costCenterService.AddCC_Type_Template(ccTemplate);
                }
                //�������true���򴴽�����³ɱ����ĳɹ�������ʧ�ܲ��׳�����
               if (result.IsSuccess == true)
               {
                   ShowResult = ShowResult.Yes;
                   if (string.IsNullOrEmpty(CTempID) == true)
                   {
                       Close();
                   }
                   Toast("�ɱ�����ģ�屣��ɹ���", ToastLength.SHORT);
               }
               else
               {
                   throw new Exception(result.ErrorInfo);
               }
            }
            catch (Exception ex)
            {
                Toast(ex.Message, ToastLength.SHORT);
            }

        }

      
        /// <summary>
        /// ���ɾ�������������¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        private void btnDelCheckClick(object sender, EventArgs e)
        {
            try
            {
               object AEACheck = ((MobileControl)sender).Tag;//��ȡ����������ͷ��
               if (AEACheck != null) 
                {
                    listAEAChecks.Remove(AEACheck.ToString());//ɾ������������
                    foreach (ImageButton imgbtnAEACheck in listbtnAEAChecksP)
                        {
                            if (imgbtnAEACheck.Name.Equals("imgbtnAEACheck" + AEACheck))
                            {
                                listbtnAEAChecksP.Remove(imgbtnAEACheck);//ɾ����������ͷ��ؼ�
                                Controls.Remove((MobileControl)imgbtnAEACheck);//ɾ����������������ͷ��ؼ�
                                break;
                            }
                        }
                    foreach (Button btnAEACheck in listbtnAEAChecks)
                        {
                            if (btnAEACheck.Name.Equals("btnAEACheck" + AEACheck))
                            {
                                listbtnAEAChecks.Remove(btnAEACheck);//ɾ�������������ƿؼ�
                                Controls.Remove((MobileControl)btnAEACheck);//ɾ�������������������ƿؼ�
                                break;
                            }
                        }
                        AEACheckTop = lblAEACheckers.Top + lblAEACheckers.Height;
                        AEAChecksSort();//����������ؿؼ�����
                        AEACheck = null;
           
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// ���ɾ�������������¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        private void btnDelFCheckClick(object sender, EventArgs e)
        {
            try
            {
                object FCheck = ((MobileControl)sender).Tag;//��ȡ����������ͷ��
                if (FCheck != null) 
                {
                    listFCheckers.Remove(FCheck.ToString());//ɾ������������
                        foreach (ImageButton imgbtnFChecker in listbtnFCheckersP)
                        {
                            if (imgbtnFChecker.Name.Equals("imgbtnFCheck" + FCheck))
                            {
                                listbtnFCheckersP.Remove(imgbtnFChecker);//ɾ����������ͷ��ؼ�
                                Controls.Remove((MobileControl)imgbtnFChecker);//ɾ�������в�������ͷ��ؼ�
                                break;
                            }
                          
                        }
                        foreach (Button btnFChecker in listbtnFCheckers)
                        {
                            if (btnFChecker.Name.Equals("btnFCheck" + FCheck))
                            {
                                listbtnFCheckers.Remove(btnFChecker);//ɾ�������������ƿؼ�
                                Controls.Remove((MobileControl)btnFChecker);//ɾ�������в����������ƿؼ�
                                break;
                            }
                        }
                        AEACheckTop = lblAEACheckers.Top + lblAEACheckers.Height;
                        AEAChecksSort();//����������ؿؼ�����
                        FCheck = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        /// <summary>
        /// ��������������
        /// </summary>
        private void addAEACheckers()
        {
            if (addAEACheck.Trim().Length > 0 & listAEAChecks.Count <= 4)
            {
                if (listAEAChecks.Contains(addAEACheck.Split(',')[0]) == false)
                {
                    listAEAChecks.Add(addAEACheck.Split(',')[0]);
                    int imgCheckWSize = 35;
                    ImageButton imgbtn = new ImageButton();
                    if (string.IsNullOrEmpty(addAEACheck.Split(',')[2]) == true)
                    {
                        UserDetailDto user = AutofacConfig.userService.GetUserByUserID(addAEACheck.Split(',')[0]);
                        switch (user.U_Sex)
                        {
                            case (int)Sex.��:
                                imgbtn.ResourceID = "boy";
                                break;
                            case (int)Sex.Ů:
                                imgbtn.ResourceID = "girl";
                                break;
                        }
                    }
                    else
                    {
                        imgbtn.ResourceID = addAEACheck.Split(',')[2];
                    }
                   

                    imgbtn.Width = imgCheckWSize;
                    imgbtn.Height = imgCheckWSize;
                    imgbtn.ZIndex = (Controls.Count + 1);
                    imgbtn.BorderRadius = 10;
                    imgbtn.Name = "imgbtnAEACheck" + addAEACheck.Split(',')[0];
                    imgbtn.SizeMode = Smobiler.Core.ImageSizeMode.StretchImage;
                    imgbtn.Tag = addAEACheck.Split(',')[0];
                    Controls.Add(imgbtn);//������������������ͷ��ؼ�
                    listbtnAEAChecksP.Add(imgbtn);//��������������ͷ��ؼ�
                    imgbtn.Click += btnDelCheckClick;//ɾ�������������¼�

                    Button btn = new Button();
                    btn.Text = addAEACheck.Split(',')[1];
                    btn.Name = "btnAEACheck" + addAEACheck.Split(',')[0];
                    btn.Width = imgCheckWSize;
                    btn.Height = 20;
                    btn.BackColor = System.Drawing.Color.White;
                    btn.ForeColor = System.Drawing.Color.FromArgb(44, 44, 44);
                    btn.FontSize = 10;
                    btn.Tag = addAEACheck.Split(',')[0];
                    btn.ZIndex = (Controls.Count + 1);
                    Controls.Add(btn);//���������������������ƿؼ�
                    listbtnAEAChecks.Add(btn);//�����������������ƿؼ�
                    btn.Click += btnDelCheckClick;//ɾ�������������¼�

                }

                addAEACheck = "";

            }
            AEAChecksSort();
        }

        /// <summary>
        /// ���Ӳ���������
        /// </summary>
        private void addFCheckers()
        {

            if (addFCheck.Trim().Length > 0 & listFCheckers.Count <= 4)
            {
                if (listFCheckers.Contains(addFCheck.Split(',')[0]) == false)
                {
                    listFCheckers.Add(addFCheck.Split(',')[0]);
                    int imgFCWSize = 35;
                    ImageButton imgbtn = new ImageButton();
                    if (string.IsNullOrEmpty(addFCheck.Split(',')[2]) == true)
                    {
                        UserDetailDto user = AutofacConfig.userService.GetUserByUserID(addFCheck.Split(',')[0]);
                        switch (user.U_Sex)
                        {
                            case (int)Sex.��:
                                imgbtn.ResourceID = "boy";
                                break;
                            case (int)Sex.Ů:
                                imgbtn.ResourceID = "girl";
                                break;
                        }
                    }
                    else
                    {
                        imgbtn.ResourceID = addFCheck.Split(',')[2];
                    }
                    imgbtn.Width = imgFCWSize;
                    imgbtn.Height = imgFCWSize;
                    imgbtn.ZIndex = (Controls.Count + 1);
                    imgbtn.BorderRadius = 10;
                    imgbtn.Name = "imgbtnFCheck" + addFCheck.Split(',')[0];
                    imgbtn.SizeMode = Smobiler.Core.ImageSizeMode.StretchImage;
                    imgbtn.Tag = addFCheck.Split(',')[0];
                    Controls.Add(imgbtn);//�������Ӳ���������ͷ��ؼ�
                    listbtnFCheckersP.Add(imgbtn);//���Ӳ���������ͷ��ؼ�
                    imgbtn.Click += btnDelFCheckClick;//ɾ�������������¼�
                  
                    Button btn = new Button();
                    btn.Text = addFCheck.Split(',')[1];
                    btn.Name = "btnFCheck" + addFCheck.Split(',')[0];
                    btn.Width = imgFCWSize;
                    btn.Height = 20;
                    btn.BackColor = System.Drawing.Color.White;
                    btn.ForeColor = System.Drawing.Color.FromArgb(44, 44, 44);
                    btn.FontSize = 10;
                    btn.Tag = addFCheck.Split(',')[0];
                    btn.ZIndex = (Controls.Count + 1);
                    Controls.Add(btn);//�������Ӳ������������ƿؼ�
                    listbtnFCheckers.Add(btn);//���Ӳ������������ƿؼ�
                    btn.Click += btnDelFCheckClick;//ɾ�������������¼�
                    
                }

                addFCheck = "";

            }
            FCheckersSort();
        }
        /// <summary>
        /// ���������˿ؼ�����
        /// </summary>
        private void AEAChecksSort()
        {
            int imgCheckWSize = 35;
            int imgCheckHSize = 55;
            imgCheckLeft = 65;
            if (listAEAChecks.Count > 0 & listAEAChecks.Count <= 4)
            {
                if (listAEAChecks.Count == 4)
                {
                    imgbtnAEACheckers.Visible = false;
                }
                else
                {
                    imgbtnAEACheckers.Visible = true;
                }
                foreach (string checkuser in listAEAChecks)
                {
                    foreach (Button btnAEACheck in listbtnAEAChecks)
                    {
                        if (btnAEACheck.Name.Equals("btnAEACheck" + checkuser))
                        {
                                if ((imgCheckLeft + imgCheckWSize) > 300)
                                {
                                    imgCheckLeft = 0;
                                    AEACheckTop = AEACheckTop + imgCheckHSize;
                                    if (AEACheckTop > Height)
                                    {
                                        Height = Height + imgCheckHSize;
                                    }
                                }
                             
                                foreach (ImageButton imgbtnAEACheck in listbtnAEAChecksP)
                                {
                                    if (imgbtnAEACheck.Name.Equals("imgbtnAEACheck" + checkuser))
                                    {
                                        imgbtnAEACheck.Left = imgCheckLeft;
                                        imgbtnAEACheck.Top = AEACheckTop;

                                        btnAEACheck.Left = imgCheckLeft;
                                        btnAEACheck.Top = imgbtnAEACheck.Top + imgbtnAEACheck.Height;
                                        imgCheckLeft += (imgCheckWSize + 10);
                                        break;
                                    }
                                }
                            continue;
                        }
                    }
                }

            }
            imgbtnAEACheckers.Top = AEACheckTop;
            imgbtnAEACheckers.Left = imgCheckLeft;
            lblFCheckers.Top = lblAEACheckers2.Top + lblAEACheckers2.Height + 10;
            lblFCheckers1.Top = lblFCheckers.Top;
            FCheckTop = lblFCheckers.Top + lblFCheckers.Height;
            lblFCheckers2.Top = FCheckTop;
            imgbtnFCheckers.Top = FCheckTop;
            FCheckersSort();
        }

        /// <summary>
        /// ���������˿ؼ�����
        /// </summary>
        private void FCheckersSort()
        {
            int imgCCToUserWSize = 35;
            int imgCCToUserHSize = 55;
            imgFCheckLeft=65;
            if (listFCheckers.Count > 0 & listFCheckers.Count <= 4)
            {
                if (listFCheckers.Count == 4)
                {
                    imgbtnFCheckers.Visible = false;
                }
                else
                {
                    imgbtnFCheckers.Visible = true;
                }
                foreach (string ccToUser in listFCheckers)
                {
                    foreach (Button btnFChecker in listbtnFCheckers)
                    {
                        if (btnFChecker.Name.Equals("btnFCheck" + ccToUser))
                        {
                           
                                if ((imgFCheckLeft + imgCCToUserWSize) > 300)
                                {
                                    imgFCheckLeft = 0;
                                    FCheckTop = FCheckTop + imgCCToUserHSize;
                                    if (FCheckTop > Height)
                                    {
                                        Height = Height + imgCCToUserHSize;
                                    }
                                }
                                foreach (ImageButton imgbtnFCCheck in listbtnFCheckersP)
                                {
                                    if (imgbtnFCCheck.Name.Equals("imgbtnFCheck" + ccToUser))
                                    {
                                        imgbtnFCCheck.Left = imgFCheckLeft;
                                        imgbtnFCCheck.Top = FCheckTop;

                                        btnFChecker.Left = imgFCheckLeft;
                                        btnFChecker.Top = imgbtnFCCheck.Top + imgbtnFCCheck.Height;
                                        imgFCheckLeft += (imgCCToUserWSize + 10);
                                        break;
                                    }
                                }
                               
                            continue;
                        }
                    }
                }
            }
            imgbtnFCheckers.Top = FCheckTop;
            imgbtnFCheckers.Left = imgFCheckLeft;
            btnSave.Top = lblFCheckers2.Top + lblFCheckers2.Height + 10;
            if (Height < (btnSave.Top + btnSave.Height))
            {
                Height = btnSave.Top + btnSave.Height + 10;
            }
        }

        /// <summary>
        /// popType���͸�ֵ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popType_Selected(object sender, EventArgs e)
        {
            if (popType.Selection != null)
            {
                type = popType.Selection.Value;
                btnType.Text = popType.Selection.Text;
            }
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgbtnAEACheckers_Click(object sender, EventArgs e)
        {
            if (listAEAChecks.Count >= 4)
            {
                Toast("���ֻ������4λ���������ˣ�", ToastLength.SHORT);
            }
            else
            {
                frmCheckOrCCTo frm = new frmCheckOrCCTo();
                frm.isCTemUser = true;
                Redirect(frm, (MobileForm form, object args) =>
                {
                    if (frm.ShowResult == Smobiler.Core.ShowResult.Yes)
                    {
                        if (string.IsNullOrWhiteSpace(frm.userInfo) == false)
                        {
                            string Check = frm.userInfo;
                            if (listAEAChecks.Contains(Check.Split(',')[0]) == true)
                            {

                                Toast("������������" + Check.Split(',')[1] + "�����б��У�");
                            }
                            else
                            {
                                addAEACheck = Check;
                                addAEACheckers();
                            }
                        }
                    }
                });
            }
        }
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgbtnFCheckers_Click(object sender, EventArgs e)
        {
            if (listFCheckers.Count >= 4)
            {
                Toast("���ֻ������4λ���������ˣ�", ToastLength.SHORT);
            }
            else
            {
                frmCheckOrCCTo frm = new frmCheckOrCCTo();
                frm.isCTemUser = true;
                Redirect(frm, (MobileForm form, object args) =>
                {
                    if (frm.ShowResult == Smobiler.Core.ShowResult.Yes)
                    {
                        if (string.IsNullOrWhiteSpace(frm.userInfo ) == false)
                        {
                            string Check = frm.userInfo;
                            if (listFCheckers.Contains(Check.Split(',')[0]) == true)
                            {

                                Toast("�ò���������" + Check.Split(',')[1] + "�����б��У�");
                            }
                            else
                            {
                                addFCheck = Check;
                                addFCheckers();
                            }
                        }
                    }
                });
            }
        }
        /// <summary>
        /// ��ʼ���¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCostTempletCreate_Load(object sender, EventArgs e)
        {
            AEACheckTop = lblAEACheckers.Top + lblAEACheckers.Height;
            FCheckTop = lblFCheckers.Top + lblFCheckers.Height;
            Bind();
        }
        /// <summary>
        /// ��ʼ������
        /// </summary>
        private void Bind()
        {
            try
            {
                if (CTempID != null)
                {
                    //���ݳɱ�����ģ���Ż�ȡ�ɱ�����ģ������
                    CC_Type_TemplateDto ccTemplate = AutofacConfig.costCenterService.GetTemplateByID(CTempID);
                    type = ccTemplate.CC_TT_TypeID;
                    btnType.Text = ccTemplate.CC_TT_TypeName;
                    if (string.IsNullOrEmpty(ccTemplate.CC_TT_AEACheckers) == false)
                    {
                        string[] CheckUsers = ccTemplate.CC_TT_AEACheckers.Split(',');
                        foreach (string checkUser in CheckUsers)
                        {
                            UserDetailDto user = AutofacConfig.userService.GetUserByUserID(checkUser);
                            addAEACheck = checkUser + "," + user.U_Name + "," + user.U_Portrait;
                            addAEACheckers();
                        }
                    }
                    if (string.IsNullOrEmpty(ccTemplate.CC_TT_FinancialCheckers) == false)
                    {
                        string[] CCToUsers = ccTemplate.CC_TT_FinancialCheckers.Split(',');
                        foreach (string ccToUser in CCToUsers)
                        {
                            UserDetailDto user = AutofacConfig.userService.GetUserByUserID(ccToUser);
                            addFCheck = ccToUser + "," + user.U_Name + "," + user.U_Portrait;
                            addFCheckers();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// �ֻ��Դ����˰�ť�¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCostTempletCreate_KeyDown(object sender, KeyDownEventArgs e)
        {
            if (e.KeyCode == KeyCode.Back)
            {
                Close();         //�رյ�ǰҳ��
            }
        }

    
        /// <summary>
        /// ������ͼƬ��ť����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCostTempletCreate_TitleImageClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}