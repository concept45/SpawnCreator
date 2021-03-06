﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace SpawnCreator
{
    public partial class GameObject_Creator : Form
    {

        // This code allow the use to minimize the form by clicking the taskbar icon of the form
        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_MINIMIZEBOX = 0x00020000;
                var cp = base.CreateParams;
                cp.Style |= WS_MINIMIZEBOX;
                return cp;
            }
        }

        public GameObject_Creator()
        {
            InitializeComponent();
        }

        private readonly Form_MainMenu form_MM;
        public GameObject_Creator(Form_MainMenu form_MainMenu)
        {
            InitializeComponent();
            form_MM = form_MainMenu;
        }

        MySqlConnection connection = new MySqlConnection();
        //MySql_Connect mysql_Connect = new MySql_Connect();

        public void GetMySqlConnection()
        {
            string connStr = string.Format("Server={0};Port={1};UID={2};Pwd={3};", form_MM.GetHost(), form_MM.GetPort(), form_MM.GetUser(), form_MM.GetPass());
            
            MySqlConnection _connection = new MySqlConnection(
                               //$"server={ form_MM.GetHost() };port={ form_MM.GetPort() };uid={ form_MM.GetUser() };pwd={ form_MM.GetPass() };"
                               connStr
                            );

            connection = _connection;
        }

        public void SelectMaxPlus1_GO()
        {
            GetMySqlConnection();

            string query = $"SELECT max(entry) + 1 FROM { form_MM.GetWorldDB() }.gameobject_template;";
            MySqlCommand _command = new MySqlCommand(query, connection);

            try
            {
                connection.Open();
                NUD_Entry.Text = _command.ExecuteScalar().ToString();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "SpawnCreator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        public void DeleteGO()
        {
            GetMySqlConnection();

            try
            {
                connection.Open();

                MySqlCommand _command = new MySqlCommand();
                _command.Connection = connection;

                _command.CommandText = $"DELETE FROM { form_MM.GetWorldDB() }.gameobject_template WHERE entry=@Entry;";
                _command.Prepare();
                _command.Parameters.AddWithValue("@Entry", NUD_Entry.Text);

                _command.ExecuteNonQuery();
                MessageBox.Show("GameObject successfully deleted! \t", "SpawnCreator", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "SpawnCreator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        public static string stringSqlShare;

        private void GenerateQuery()
        {
            string BuildSqlFile;
            
            BuildSqlFile = $" { textBox105.Text } INTO { form_MM.GetWorldDB() }.gameobject_template ";
            BuildSqlFile += "(entry, type, displayId, name, IconName, castBarCaption, unk1, ";
            BuildSqlFile += "size, Data0, Data1, Data2, Data3, Data4, Data5, Data6, Data7, Data8, Data9, Data10, Data11, Data12, Data13, Data14, Data15, ";
            BuildSqlFile += "Data16, Data17, Data18, Data19, Data20, Data21, Data22, Data23, AIName, ScriptName, VerifiedBuild) ";
            BuildSqlFile += "VALUES " + Environment.NewLine;

            

            BuildSqlFile += $"({ NUD_Entry.Value }, "; // Entry

            KeyValuePair<int, string> selectedPair = (KeyValuePair<int, string>)comboBox2.SelectedItem;
            BuildSqlFile += $"{ selectedPair.Key }, "; // Type

            //looping through all TextBoxes
            //foreach (TextBox txtBox in groupBox1.Controls.OfType<TextBox>())
            //{
            //    if (string.IsNullOrWhiteSpace(txtBox.Text))
            //    {
            //        BuildSqlFile += "0, ";
            //    }
            //}

            if (textBox3.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{ textBox3.Text }, "; // displayId
            BuildSqlFile += $"'{ textBox2.Text }', "; // name
            BuildSqlFile += $"'{ comboBox1.Text }', "; // IconName
            BuildSqlFile += $"'{ textBox4.Text }', "; // castBarCaption
            BuildSqlFile += $"'{textBox28.Text }', ";// unk1
            if (textBox34.Text == "") BuildSqlFile += "1, ";
            else
                BuildSqlFile += $"{textBox34.Text }, "; // size
            if (textBox26.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox26.Text }, "; // Data0
            if (textBox7.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox7.Text }, "; // Data1
            if (textBox18.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox18.Text }, "; // Data2
            if (textBox29.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox29.Text }, "; // Data3
            if (textBox5.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox5.Text }, "; // Data4
            if (textBox20.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox20.Text }, "; // Data5
            if (textBox27.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox27.Text }, "; // Data6
            if (textBox6.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox6.Text }, "; // Data7
            if (textBox19.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox19.Text }, "; // Data8
            if (textBox25.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox25.Text }, "; // Data9
            if (textBox8.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox8.Text }, "; // Data10
            if (textBox17.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox17.Text }, "; // Data11
            if (textBox22.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox22.Text }, "; // Data12
            if (textBox10.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox10.Text }, "; // Data13
            if (textBox14.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox14.Text }, "; // Data14
            if (textBox24.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox24.Text }, "; // Data15
            if (textBox12.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox12.Text }, "; // Data16
            if (textBox16.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox16.Text }, "; // Data17
            if (textBox23.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox23.Text }, "; // Data18
            if (textBox11.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox11.Text }, "; // Data19
            if (textBox15.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox15.Text }, "; // Data20
            if (textBox21.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox21.Text }, "; // Data21
            if (textBox9.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox9.Text }, "; // Data22
            if (textBox13.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{textBox13.Text }, "; // Data23
            BuildSqlFile += $"'{ comboBox3.Text }', "; // AiName
            BuildSqlFile += $"'{ textBox30.Text }', "; // ScriptName
            if (textBox32.Text == "") BuildSqlFile += "0, ";
            else
                BuildSqlFile += $"{ textBox32.Text }); ";  // VerifiedBuild

            stringSqlShare = BuildSqlFile;
        }

        public bool IsProcessOpen(string name = "mysqld")
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(name))
                {
                    label_mysql_status2.Text = "Connected!";
                    label_mysql_status2.ForeColor = Color.LawnGreen;
                    button_maxPlus1fromDB.Visible = true;
                    button_execute_query.Visible = true;
                    btn_DeleteQuery.Enabled = true;

                    // Start with mysql
                    label_mysql_status2.Visible = true;
                    label85.Visible = true;
                    timer1.Enabled = true;
                    return true;
                }
            }

            label_mysql_status2.Text = "Connection Lost - MySQL is not running";
            label_mysql_status2.ForeColor = Color.Red;
            button_maxPlus1fromDB.Visible = false;
            button_execute_query.Visible = false;
            btn_DeleteQuery.Enabled = false;
            return false;
        }

        private bool _mouseDown;
        private Point lastLocation;

        Form_MainMenu mainmenu = new Form_MainMenu();

        //private void SelectMaxPlusOne()
        //{
        //    //GetMySqlConnection();

        //    string insertQuery = $"SELECT max(entry)+1 FROM { form_MM.GetWorldDB() } .gameobject_template;";
        //    connection.Open();
        //    MySqlCommand command = new MySqlCommand(insertQuery, connection);

        //    try
        //    {
        //        NUD_Entry.Value = Convert.ToDecimal(command.ExecuteScalar());
        //    }
        //    catch (MySqlException ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    connection.Close();
        //}

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex      == 0) pictureBox1.Image = Properties.Resources.Taxi;
            else if (comboBox1.SelectedIndex == 1) pictureBox1.Image = Properties.Resources.Speak;
            else if (comboBox1.SelectedIndex == 2) pictureBox1.Image = Properties.Resources.Attack;
            else if (comboBox1.SelectedIndex == 3) pictureBox1.Image = Properties.Resources.Directions;
            else if (comboBox1.SelectedIndex == 4) pictureBox1.Image = Properties.Resources.Quest;
        }

        private void GameObject_Creator_Load(object sender, EventArgs e)
        {
            // Create a List to store our KeyValuePairs
            List<KeyValuePair<int, string>> data = new List<KeyValuePair<int, string>>();

            // Add data to the list
            data.Add(new KeyValuePair<int, string>(0, "0 - GAMEOBJECT_TYPE_DOOR"));
            data.Add(new KeyValuePair<int, string>(1, "1 - GAMEOBJECT_TYPE_BUTTON"));
            data.Add(new KeyValuePair<int, string>(2, "2 - GAMEOBJECT_TYPE_QUESTGIVER"));
            data.Add(new KeyValuePair<int, string>(3, "3 - GAMEOBJECT_TYPE_CHEST"));
            data.Add(new KeyValuePair<int, string>(4, "4 - GAMEOBJECT_TYPE_BINDER"));
            data.Add(new KeyValuePair<int, string>(5, "5 - GAMEOBJECT_TYPE_GENERIC"));
            data.Add(new KeyValuePair<int, string>(6, "6 - GAMEOBJECT_TYPE_TRAP"));
            data.Add(new KeyValuePair<int, string>(7, "7 - GAMEOBJECT_TYPE_CHAIR"));
            data.Add(new KeyValuePair<int, string>(8, "8 - GAMEOBJECT_TYPE_SPELL_FOCUS"));
            data.Add(new KeyValuePair<int, string>(9, "9 - GAMEOBJECT_TYPE_TEXT"));
            data.Add(new KeyValuePair<int, string>(10, "10 - GAMEOBJECT_TYPE_GOOBER"));
            data.Add(new KeyValuePair<int, string>(11, "11 - GAMEOBJECT_TYPE_TRANSPORT"));
            data.Add(new KeyValuePair<int, string>(12, "12 - GAMEOBJECT_TYPE_AREADAMAGE"));
            data.Add(new KeyValuePair<int, string>(13, "13 - GAMEOBJECT_TYPE_CAMERA"));
            data.Add(new KeyValuePair<int, string>(14, "14 - GAMEOBJECT_TYPE_MAP_OBJECT"));
            data.Add(new KeyValuePair<int, string>(15, "15 - GAMEOBJECT_TYPE_MO_TRANSPORT"));
            data.Add(new KeyValuePair<int, string>(16, "16 - GAMEOBJECT_TYPE_DUEL_ARBITER"));
            data.Add(new KeyValuePair<int, string>(17, "17 - GAMEOBJECT_TYPE_FISHINGNODE"));
            data.Add(new KeyValuePair<int, string>(18, "18 - GAMEOBJECT_TYPE_RITUAL"));
            data.Add(new KeyValuePair<int, string>(19, "19 - GAMEOBJECT_TYPE_MAILBOX"));
            data.Add(new KeyValuePair<int, string>(20, "20 - GAMEOBJECT_TYPE_AUCTIONHOUSE"));
            data.Add(new KeyValuePair<int, string>(21, "21 - GAMEOBJECT_TYPE_GUARDPOST"));
            data.Add(new KeyValuePair<int, string>(22, "22 - GAMEOBJECT_TYPE_SPELLCASTER"));
            data.Add(new KeyValuePair<int, string>(23, "23 - GAMEOBJECT_TYPE_MEETINGSTONE"));
            data.Add(new KeyValuePair<int, string>(24, "24 - GAMEOBJECT_TYPE_FLAGSTAND"));
            data.Add(new KeyValuePair<int, string>(25, "25 - GAMEOBJECT_TYPE_FISHINGHOLE"));
            data.Add(new KeyValuePair<int, string>(26, "26 - GAMEOBJECT_TYPE_FLAGDROP"));
            data.Add(new KeyValuePair<int, string>(27, "27 - GAMEOBJECT_TYPE_MINI_GAME"));
            data.Add(new KeyValuePair<int, string>(28, "28 - GAMEOBJECT_TYPE_LOTTERY_KIOSK"));
            data.Add(new KeyValuePair<int, string>(29, "29 - GAMEOBJECT_TYPE_CAPTURE_POINT"));
            data.Add(new KeyValuePair<int, string>(30, "30 - GAMEOBJECT_TYPE_AURA_GENERATOR"));
            data.Add(new KeyValuePair<int, string>(31, "31 - GAMEOBJECT_TYPE_DUNGEON_DIFFICULTY"));
            data.Add(new KeyValuePair<int, string>(32, "32 - GAMEOBJECT_TYPE_BARBER_CHAIR"));
            data.Add(new KeyValuePair<int, string>(33, "33 - GAMEOBJECT_TYPE_DESTRUCTIBLE_BUILDING"));
            data.Add(new KeyValuePair<int, string>(34, "34 - GAMEOBJECT_TYPE_GUILD_BANK"));
            data.Add(new KeyValuePair<int, string>(35, "35 - GAMEOBJECT_TYPE_TRAPDOOR"));

            // Bind the combobox
            comboBox2.DataSource = new BindingSource(data, null);
            comboBox2.DisplayMember = "Value";
            comboBox2.ValueMember = "Key";

            comboBox1.SelectedIndex = 1; // Speak
            comboBox2.SelectedIndex = 0; // GAMEOBJECT_TYPE_DOOR
            comboBox11.SelectedIndex = 0; // INSERT
            timer1.Start(); // check if mysql is running
            timer2.Start(); // stopwatch


            if (form_MM.CB_NoMySQL.Checked)
            {
                label_mysql_status2.Visible = false;
                label85.Visible = false;
                timer1.Enabled = false;
                button_maxPlus1fromDB.Visible = false;
                button_execute_query.Visible = false;
            }
            else
                SelectMaxPlus1_GO();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeyValuePair<int, string> selectedPair = (KeyValuePair<int, string>)comboBox2.SelectedItem;

            switch (selectedPair.Key)
            {
                case 0: // GAMEOBJECT_TYPE_DOOR
                    RTB_Notes.Text = "data0: startOpen(Boolean flag)" + Environment.NewLine +
                                 "data1: open(LockId from Lock.dbc)" + Environment.NewLine +
                                 "data2: autoClose(Time in milliseconds)" + Environment.NewLine +
                                 "data3: noDamageImmune(Boolean flag)" + Environment.NewLine +
                                 "data4: openTextID(Unknown Text ID)" + Environment.NewLine +
                                 "data5: closeTextID(Unknown Text ID)" + Environment.NewLine +
                                 "data6: Ignored by pathfinding" + Environment.NewLine +
                                 "data7: Conditionid1" + Environment.NewLine +
                                 "data8: Door is opaque" + Environment.NewLine +
                                 "data9: Gigantic AOI" + Environment.NewLine +
                                 "data10: Infinite AOI";
                    break;
                case 1: // GAMEOBJECT_TYPE_BUTTON
                    RTB_Notes.Text = "data0: startOpen (State)" + Environment.NewLine +
                                 "data1: open(LockId from Lock.dbc)" + Environment.NewLine +
                                 "data2: autoClose(long unknown flag)" + Environment.NewLine +
                                 "data3: linkedTrap(gameobject_template.entry(Spawned GO type 6))" + Environment.NewLine +
                                 "data4: noDamageImmune(Boolean flag)" + Environment.NewLine +
                                 "data5: large ? (Boolean flag)" + Environment.NewLine +
                                 "data6: openTextID(Unknown Text ID)" + Environment.NewLine +
                                 "data7: closeTextID(Unknown Text ID)" + Environment.NewLine +
                                 "data8: losOK(Boolean flag)" + Environment.NewLine +
                                 "data9: Conditionid1";
                    break;

                case 2: // GAMEOBJECT_TYPE_QUESTGIVER
                    RTB_Notes.Text = "data0: open (LockId from Lock.dbc)" + Environment.NewLine +
                                 "data1: questList (unknown ID)" + Environment.NewLine +
                                 "data2: pageMaterial (PageTextMaterial.dbc)" + Environment.NewLine +
                                 "data3: gossipID (gossip_menu_option.menu_id)" + Environment.NewLine +
                                 "data4: customAnim (unknown value from 1 to 4)" + Environment.NewLine +
                                 "data5: noDamageImmune (Boolean flag)" + Environment.NewLine +
                                 "data6: openTextID (broadcast_text ID)" + Environment.NewLine +
                                 "data7: losOK (Boolean flag)" + Environment.NewLine +
                                 "data8: allowMounted (Boolean flag)" + Environment.NewLine +
                                 "data9: large? (Boolean flag)" + Environment.NewLine +
                                 "data10: Conditionid1" + Environment.NewLine +
                                 "data11: Never usable while mounted";
                    break;
                case 3: // GAMEOBJECT_TYPE_CHEST
                    RTB_Notes.Text = "data0: open (LockId from Lock.dbc)" + Environment.NewLine +
                                 "data1: chestLoot (gameobject_loot_template.entry) WDB-fields" + Environment.NewLine +
                                 "data2: chestRestockTime (time in seconds)" + Environment.NewLine +
                                 "data3: consumable (State: Boolean flag)" + Environment.NewLine +
                                 "data4: minRestock (Min successful loot attempts for Mining, Herbalism etc)" + Environment.NewLine +
                                 "data5: maxRestock (Max successful loot attempts for Mining, Herbalism etc)" + Environment.NewLine +
                                 "data6: lootedEvent (unknown ID)" + Environment.NewLine +
                                 "data7: linkedTrap (gameobject_template.entry (Spawned GO type 6))" + Environment.NewLine +
                                 "data8: questID (quest_template.id of completed quest)" + Environment.NewLine +
                                 "data9: level (minimal level required to open this gameobject)" + Environment.NewLine +
                                 "data10: losOK (Boolean flag)" + Environment.NewLine +
                                 "data11: leaveLoot (Boolean flag)" + Environment.NewLine +
                                 "data12: notInCombat (Boolean flag)" + Environment.NewLine +
                                 "data13: log loot (Boolean flag)" + Environment.NewLine +
                                 "data14: openTextID (Unknown ID)" + Environment.NewLine +
                                 "data15: use group loot rules (Boolean flag)" + Environment.NewLine +
                                 "data16: floating tooltip" + Environment.NewLine +
                                 "data17: conditionid1" + Environment.NewLine +
                                 "data18: xplevel" + Environment.NewLine +
                                 "data19: xpDifficulty" + Environment.NewLine +
                                 "data20: lootlevel" + Environment.NewLine +
                                 "data21: Group Xp" + Environment.NewLine +
                                 "data22: Damage Immune" + Environment.NewLine +
                                 "data23: trivialSkillLow";
                    break;
                case 4: // GAMEOBJECT_TYPE_BINDER
                    RTB_Notes.Text = "Object type not used";
                    break;
                case 5: // GAMEOBJECT_TYPE_GENERIC
                    RTB_Notes.Text = "data0: floatingTooltip(Boolean flag)" + Environment.NewLine +
                                 "data1: highlight (Boolean flag)" + Environment.NewLine +
                                 "data2: serverOnly? (Always 0)" + Environment.NewLine +
                                 "data3: large? (Boolean flag)" + Environment.NewLine +
                                 "data4: floatOnWater (Boolean flag)" + Environment.NewLine +
                                 "data5: questID (Required active quest_template.id to work)" + Environment.NewLine +
                                 "data6: conditionID1" + Environment.NewLine +
                                 "data7: LargeAOI" + Environment.NewLine +
                                 "data8: UseGarrisonOwnerGuildColors";
                    break;
                case 6: // GAMEOBJECT_TYPE_TRAP
                    RTB_Notes.Text = "data0: open (LockId from Lock.dbc )" + Environment.NewLine +
                                 "data1: level (npc equivalent level for casted spell)" + Environment.NewLine +
                                 "data2: diameter (so radius * 2)" + Environment.NewLine +
                                 "data3: spell (Spell Id from Spell.dbc)" + Environment.NewLine +
                                 "data4: type (0 trap with no despawn after cast. 1 trap despawns after cast. 2 bomb casts on spawn)" + Environment.NewLine +
                                 "data5: cooldown (time in seconds)" + Environment.NewLine +
                                 "data6:  ? (unknown flag)" + Environment.NewLine +
                                 "data7: startDelay? (time in seconds)" + Environment.NewLine +
                                 "data8: serverOnly? (always 0)" + Environment.NewLine +
                                 "data9: stealthed (Boolean flag)" + Environment.NewLine +
                                 "data10: large? (Boolean flag)" + Environment.NewLine +
                                 "data11: stealthAffected (Boolean flag)" + Environment.NewLine +
                                 "data12: openTextID (Unknown ID)" + Environment.NewLine +
                                 "data13: closeTextID" + Environment.NewLine +
                                 "data14: IgnoreTotems" + Environment.NewLine +
                                 "data15: conditionID1" + Environment.NewLine +
                                 "data16: playerCast" + Environment.NewLine +
                                 "data17: SummonerTriggered" + Environment.NewLine +
                                 "data18: requireLOS";
                    break;
                case 7: // GAMEOBJECT_TYPE_CHAIR
                    RTB_Notes.Text = "data0: chairslots(number of players that can sit down on it)" + Environment.NewLine +
                                 "data1: chairorientation? (number of usable side?)" + Environment.NewLine +
                                 "data2: onlyCreatorUse" + Environment.NewLine +
                                 "data3: triggeredEvent" + Environment.NewLine +
                                 "data4: conditionID1";
                    break;
                case 8: // GAMEOBJECT_TYPE_SPELL_FOCUS
                    RTB_Notes.Text = "data0: spellFocusType (from SpellFocusObject.dbc; value also appears as RequiresSpellFocus in Spell.dbc)" + Environment.NewLine +
                                 "data1: diameter (so radius*2)" + Environment.NewLine +
                                 "data2: linkedTrap (gameobject_template.entry (Spawned GO type 6))" + Environment.NewLine +
                                 "data3: serverOnly? (Always 0)" + Environment.NewLine +
                                 "data4: questID (Required active quest_template.id to work)" + Environment.NewLine +
                                 "data5: large? (Boolean flag)" + Environment.NewLine +
                                 "data6: floatingTooltip (Boolean flag)" + Environment.NewLine +
                                 "data7: floatOnWater" + Environment.NewLine +
                                 "data8: conditionID1";
                    break;
                case 9: // GAMEOBJECT_TYPE_TEXT
                    RTB_Notes.Text = "data0: pageID (page_text.entry)" + Environment.NewLine +
                                 "data1: language (from  Languages.dbc)" + Environment.NewLine +
                                 "data2: pageMaterial (PageTextMaterial.dbc)" + Environment.NewLine +
                                 "data3: allowMounted" + Environment.NewLine +
                                 "data4: conditionID1" + Environment.NewLine +
                                 "data5: NeverUsableWhileMounted";
                    break;
                case 10: // GAMEOBJECT_TYPE_GOOBER
                    RTB_Notes.Text = "data0: open (LockId from Lock.dbc)" + Environment.NewLine +
                                 "data1: questID (Required active quest_template.id to work)" + Environment.NewLine +
                                 "data2: eventID (event_script id)" + Environment.NewLine +
                                 "data3:  Time in ms before the initial state is restored" + Environment.NewLine +
                                 "data4: customAnim (unknown)" + Environment.NewLine +
                                 "data5: consumable (Boolean flag controling if gameobject will despawn or not)" + Environment.NewLine +
                                 "data6: cooldown (time is seconds)" + Environment.NewLine +
                                 "data7: pageID (page_text.entry)" + Environment.NewLine +
                                 "data8: language (from Languages.dbc)" + Environment.NewLine +
                                 "data9: pageMaterial (PageTextMaterial.dbc)" + Environment.NewLine +
                                 "data10: spell (Spell Id from Spell.dbc)" + Environment.NewLine +
                                 "data11: noDamageImmune (Boolean flag)" + Environment.NewLine +
                                 "data12: linkedTrap (gameobject_template.entry (Spawned GO type 6))" + Environment.NewLine +
                                 "data13: large? (Boolean flag)" + Environment.NewLine +
                                 "data14: openTextID (Unknown ID)" + Environment.NewLine +
                                 "data15: closeTextID (Unknown ID)" + Environment.NewLine +
                                 "data16: losOK (Boolean flag) (somewhat related to battlegrounds)" + Environment.NewLine +
                                 "data19: gossipID - casts the spell when used" + Environment.NewLine +
                                 "data20: AllowMultiInteract" + Environment.NewLine +
                                 "data21: floatOnWater" + Environment.NewLine +
                                 "data22: conditionID1" + Environment.NewLine +
                                 "data23: playerCast";
                    break;
                case 11: // GAMEOBJECT_TYPE_TRANSPORT
                    RTB_Notes.Text = "data0: Timeto2ndfloor" + Environment.NewLine +
                                 "data1: startOpen" + Environment.NewLine +
                                 "data2: autoClose" + Environment.NewLine +
                                 "data3: Reached1stfloor" + Environment.NewLine +
                                 "data4: Reached2ndfloor" + Environment.NewLine +
                                 "data5: SpawnMap" + Environment.NewLine +
                                 "data6: Timeto3rdfloor" + Environment.NewLine +
                                 "data7: Reached3rdfloor" + Environment.NewLine +
                                 "data8: Timeto4rdfloor" + Environment.NewLine +
                                 "data9: Reached4rdfloor" + Environment.NewLine +
                                 "data10: Timeto5rdfloor" + Environment.NewLine +
                                 "data11: Reached5rdfloor" + Environment.NewLine +
                                 "data12: Timeto6rdfloor" + Environment.NewLine +
                                 "data13: Reached6rdfloor" + Environment.NewLine +
                                 "data14: Timeto7rdfloor" + Environment.NewLine +
                                 "data15: Reached7rdfloor" + Environment.NewLine +
                                 "data16: Timeto8rdfloor" + Environment.NewLine +
                                 "data17: Reached8rdfloor" + Environment.NewLine +
                                 "data18: Timeto9rdfloor" + Environment.NewLine +
                                 "data19: Reached9rdfloor" + Environment.NewLine +
                                 "data20: Timeto10rdfloor" + Environment.NewLine +
                                 "data21: Reached10rdfloor" + Environment.NewLine +
                                 "data22: onlychargeheightcheck" + Environment.NewLine +
                                 "data23: onlychargetimecheck";
                    break;
                case 12: // GAMEOBJECT_TYPE_AREADAMAGE
                    RTB_Notes.Text = "data0: open" + Environment.NewLine +
                                 "data1: radius" + Environment.NewLine +
                                 "data2: damageMin" + Environment.NewLine +
                                 "data3: damageMax" + Environment.NewLine +
                                 "data4: damageSchool" + Environment.NewLine +
                                 "data5: autoClose" + Environment.NewLine +
                                 "data6: openTextID" + Environment.NewLine +
                                 "data7: closeTextID";
                    break;
                case 13: // GAMEOBJECT_TYPE_CAMERA
                    RTB_Notes.Text = "data0: open (LockId from Lock.dbc)" + Environment.NewLine +
                                 "data1: camera (Cinematic entry from CinematicCamera.dbc)" + Environment.NewLine +
                                 "data2: eventID" + Environment.NewLine +
                                 "data3: openTextID" + Environment.NewLine +
                                 "data4: conditionID1";
                    break;
                case 14: // GAMEOBJECT_TYPE_MAP_OBJECT
                    RTB_Notes.Text = "No data used, all are always 0";
                    break;
                case 15: // GAMEOBJECT_TYPE_MO_TRANSPORT
                    RTB_Notes.Text = "data0: taxiPathID (Id from TaxiPath.dbc)" + Environment.NewLine +
                                 "data1: moveSpeed" + Environment.NewLine +
                                 "data2: accelRate" + Environment.NewLine +
                                 "data3: startEventID" + Environment.NewLine +
                                 "data4: stopEventID" + Environment.NewLine +
                                 "data5: transportPhysics" + Environment.NewLine +
                                 "data6: SpawnMap" + Environment.NewLine +
                                 "data7: worldState1" + Environment.NewLine +
                                 "data8: allowstopping" + Environment.NewLine +
                                 "data9: InitStopped" + Environment.NewLine +
                                 "data10: TrueInfiniteAOI";
                    break;
                case 16: // GAMEOBJECT_TYPE_DUEL_ARBITER
                    RTB_Notes.Text = "Only one Gameobject with this type (21680 - Duel Flag) and no data";
                    break;
                case 17: // GAMEOBJECT_TYPE_FISHINGNODE
                    RTB_Notes.Text = "Only one Gameobject with this type (35591 - Fishing Bobber) and no data";
                    break;
                case 18: // GAMEOBJECT_TYPE_RITUAL
                    RTB_Notes.Text = "data0: casters?" + Environment.NewLine +
                                 "data1: spell (Spell Id from Spell.dbc)" + Environment.NewLine +
                                 "data2: animSpell (Spell Id from Spell.dbc)" + Environment.NewLine +
                                 "data3: ritualPersistent (Boolean flag)" + Environment.NewLine +
                                 "data4: casterTargetSpell (Spell Id from Spell.dbc)" + Environment.NewLine +
                                 "data5: casterTargetSpellTargets (Boolean flag)" + Environment.NewLine +
                                 "data6: castersGrouped (Boolean flag)" + Environment.NewLine +
                                 "data7: ritualNoTargetCheck" + Environment.NewLine +
                                 "data8: conditionID1";
                    break;
                case 19: // GAMEOBJECT_TYPE_MAILBOX
                    RTB_Notes.Text = "No data used, all are always 0";
                    break;
                case 20: // GAMEOBJECT_TYPE_AUCTIONHOUSE
                    RTB_Notes.Text = "data0: actionHouseID (From AuctionHouse.dbc ?)";
                    break;
                case 21: // GAMEOBJECT_TYPE_GUARDPOST
                    RTB_Notes.Text = "data0: CreatureID" + Environment.NewLine +
                                 "data1: unk";
                    break;
                case 22: // GAMEOBJECT_TYPE_SPELLCASTER
                    RTB_Notes.Text = "data0: spell (Spell Id from Spell.dbc)" + Environment.NewLine +
                                 "data1: charges" + Environment.NewLine +
                                 "data2: partyOnly (Boolean flag, need to be in group to use it)" + Environment.NewLine +
                                 "data3: allowMounted" + Environment.NewLine +
                                 "data4: GiganticAOI" + Environment.NewLine +
                                 "data5: conditionID1" + Environment.NewLine +
                                 "data6: playerCast" + Environment.NewLine +
                                 "data7: NeverUsableWhileMounted";
                    break;
                case 23: // GAMEOBJECT_TYPE_MEETINGSTONE
                    RTB_Notes.Text = "data0: minLevel" + Environment.NewLine +
                                 "data1: maxLevel" + Environment.NewLine +
                                 "data2: areaID (From AreaTable.dbc)";
                    break;
                case 24: // GAMEOBJECT_TYPE_FLAGSTAND
                    RTB_Notes.Text = "data0: open (LockId from Lock.dbc)" + Environment.NewLine +
                                 "data1: pickupSpell (Spell Id from Spell.dbc)" + Environment.NewLine +
                                 "data2: radius (distance)" + Environment.NewLine +
                                 "data3: returnAura (Spell Id from Spell.dbc)" + Environment.NewLine +
                                 "data4: returnSpell (Spell Id from Spell.dbc)" + Environment.NewLine +
                                 "data5: noDamageImmune (Boolean flag)" + Environment.NewLine +
                                 "data6: openTextID" + Environment.NewLine +
                                 "data7: losOK (Boolean flag)" + Environment.NewLine +
                                 "data8: conditionID1" + Environment.NewLine +
                                 "data9: playerCast" + Environment.NewLine +
                                 "data10: GiganticAOI" + Environment.NewLine +
                                 "data11: InfiniteAOI" + Environment.NewLine +
                                 "data12: cooldown";
                    break;
                case 25: // GAMEOBJECT_TYPE_FISHINGHOLE
                    RTB_Notes.Text = "data0: radius (distance)" + Environment.NewLine +
                                 "data1: chestLoot (gameobject_loot_template.entry)" + Environment.NewLine +
                                 "data2: minRestock" + Environment.NewLine +
                                 "data3: maxRestock" + Environment.NewLine +
                                 "data4: open";
                    break;
                case 26: // GAMEOBJECT_TYPE_FLAGDROP
                    RTB_Notes.Text = "data0: open (LockId from Lock.dbc)" + Environment.NewLine +
                                 "data1: eventID (Unknown Event ID)" + Environment.NewLine +
                                 "data2: pickupSpell (Spell Id from Spell.dbc)" + Environment.NewLine +
                                 "data3: noDamageImmune (Boolean flag)" + Environment.NewLine +
                                 "data4: openTextID" + Environment.NewLine +
                                 "data5: playerCast" + Environment.NewLine +
                                 "data6: ExpireDuration" + Environment.NewLine +
                                 "data7: GiganticAOI" + Environment.NewLine +
                                 "data8: InfiniteAOI" + Environment.NewLine +
                                 "data9: cooldown";
                    break;
                case 27: // GAMEOBJECT_TYPE_MINI_GAME
                    RTB_Notes.Text = "Object type not used. Reused in core for CUSTOM_TELEPORT" + Environment.NewLine +
                                 "data0: areatrigger_teleport.id";
                    break;
                case 28: // GAMEOBJECT_TYPE_LOTTERY_KIOSK
                    RTB_Notes.Text = "Object type not used";
                    break;
                case 29: // GAMEOBJECT_TYPE_CAPTURE_POINT
                    RTB_Notes.Text = "data0: radius (Distance)" + Environment.NewLine +
                                 "data1: spell (Unknown ID, not a spell id in dbc file, maybe server only side spell)" + Environment.NewLine +
                                 "data2: worldState1" + Environment.NewLine +
                                 "data3: worldstate2" + Environment.NewLine +
                                 "data4: winEventID1 (Unknown Event ID)" + Environment.NewLine +
                                 "data5: winEventID2 (Unknown Event ID)" + Environment.NewLine +
                                 "data6: contestedEventID1 (Unknown Event ID)" + Environment.NewLine +
                                 "data7: contestedEventID2 (Unknown Event ID)" + Environment.NewLine +
                                 "data8: progressEventID1 (Unknown Event ID)" + Environment.NewLine +
                                 "data9: progressEventID2 (Unknown Event ID)" + Environment.NewLine +
                                 "data10: neutralEventID1 (Unknown Event ID)" + Environment.NewLine +
                                 "data11: neutralEventID2 (Unknown Event ID)" + Environment.NewLine +
                                 "data12: neutralPercent" + Environment.NewLine +
                                 "data13: worldstate3" + Environment.NewLine +
                                 "data14: minSuperiority" + Environment.NewLine +
                                 "data15: maxSuperiority" + Environment.NewLine +
                                 "data16: minTime (in seconds)" + Environment.NewLine +
                                 "data17: maxTime (in seconds)" + Environment.NewLine +
                                 "data18: large? (Boolean flag)" + Environment.NewLine +
                                 "data19: highlight" + Environment.NewLine +
                                 "data20: startingValue" + Environment.NewLine +
                                 "data21: unidirectional" + Environment.NewLine +
                                 "data22: killbonustime" + Environment.NewLine +
                                 "data23: speedWorldState1";
                    break;
                case 30: // GAMEOBJECT_TYPE_AURA_GENERATOR
                    RTB_Notes.Text = "data0: startOpen (Boolean flag)" + Environment.NewLine +
                                 "data1: radius (Distance)" + Environment.NewLine +
                                 "data2: auraID1 (Spell Id from Spell.dbc)" + Environment.NewLine +
                                 "data3: conditionID1 (Unknown ID)" + Environment.NewLine +
                                 "data4: auraID2" + Environment.NewLine +
                                 "data5: conditionID2" + Environment.NewLine +
                                 "data6: serverOnly";
                    break;
                case 31: // GAMEOBJECT_TYPE_DUNGEON_DIFFICULTY
                    RTB_Notes.Text = "data0: mapID (From Map.dbc)" + Environment.NewLine +
                                 "data1: difficulty" + Environment.NewLine +
                                 "data2: DifficultyHeroic" + Environment.NewLine +
                                 "data3: DifficultyEpic" + Environment.NewLine +
                                 "data4: DifficultyLegendary" + Environment.NewLine +
                                 "data5: HeroicAttachment" + Environment.NewLine +
                                 "data6: ChallengeAttachment" + Environment.NewLine +
                                 "data7: DifficultyAnimations" + Environment.NewLine +
                                 "data8: LargeAOI" + Environment.NewLine +
                                 "data9: GiganticAOI" + Environment.NewLine +
                                 "data10: Legacy" + Environment.NewLine + Environment.NewLine +
                                 "-----------------------------------------------------------" + Environment.NewLine +
                                 "Value   |   Comment" + Environment.NewLine +
                                 "-----------------------------------------------------------" + Environment.NewLine +
                                 "0           |   5 man normal, 10 man normal" + Environment.NewLine +
                                 "1           |   5 man heroic, 25 normal" + Environment.NewLine +
                                 "2           |   10 man heroic" + Environment.NewLine +
                                 "3           |   25 man heroic" ;
                    break;
                case 32: // GAMEOBJECT_TYPE_BARBER_CHAIR
                    RTB_Notes.Text = "data0: chairheight" + Environment.NewLine +
                                 "data1: HeightOffset" + Environment.NewLine +
                                 "data2: SitAnimKit";
                    break;
                case 33: // GAMEOBJECT_TYPE_DESTRUCTIBLE_BUILDING
                    RTB_Notes.Text = "data0: intactNumHits" + Environment.NewLine +
                                 "data1: creditProxyCreature" + Environment.NewLine +
                                 "data2: state1Name" + Environment.NewLine +
                                 "data3: intactEvent" + Environment.NewLine +
                                 "data4: damagedDisplayId" + Environment.NewLine +
                                 "data5: damagedNumHits" + Environment.NewLine +
                                 "data6: empty3" + Environment.NewLine +
                                 "data7: empty4" + Environment.NewLine +
                                 "data8: empty5" + Environment.NewLine +
                                 "data9: damagedEvent" + Environment.NewLine +
                                 "data10: destroyedDisplayId" + Environment.NewLine +
                                 "data11: empty7" + Environment.NewLine +
                                 "data12: empty8" + Environment.NewLine +
                                 "data13: empty9" + Environment.NewLine +
                                 "data14: destroyedEvent" + Environment.NewLine +
                                 "data15: empty10" + Environment.NewLine +
                                 "data16: debuildingTimeSecs" + Environment.NewLine +
                                 "data17: empty11" + Environment.NewLine +
                                 "data18: destructibleData" + Environment.NewLine +
                                 "data19: rebuildingEvent" + Environment.NewLine +
                                 "data20: empty12" + Environment.NewLine +
                                 "data21: empty13" + Environment.NewLine +
                                 "data22: damageEvent" + Environment.NewLine +
                                 "data23: empty14";
                    break;
                case 34: // GAMEOBJECT_TYPE_GUILD_BANK
                    RTB_Notes.Text = "No data data used, all are always 0";
                    break;
                case 35: // GAMEOBJECT_TYPE_TRAPDOOR
                    RTB_Notes.Text = "data0: whenToPause" + Environment.NewLine +
                                 "data1: startOpen" + Environment.NewLine +
                                 "data2: autoClose" + Environment.NewLine +
                                 "data3: BlocksPathsDown" + Environment.NewLine +
                                 "data4: PathBlockerBump";
                    break;
            }
        }

        private void label35_Click(object sender, EventArgs e)
        {
            Process.Start("https://trinitycore.atlassian.net/wiki/display/tc/gameobject_template");
        }

        private void button_execute_query_Click(object sender, EventArgs e)
        {
            if (comboBox11.SelectedIndex == 2)
                comboBox11.SelectedIndex = 0;

            GenerateQuery();
            
            if (NUD_Entry.Text == "0")
            {
                MessageBox.Show("Entry should not be 0", "Error");
                return;
            }
            if (NUD_Entry.Text == "")
            {
                MessageBox.Show("Entry should not be empty", "Error");
                return;
            }

            //GetMySqlConnection();
            
            MySqlCommand command = new MySqlCommand(stringSqlShare, connection);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                timer3.Start();
                connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "SpawnCreator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void button_maxPlus1fromDB_Click(object sender, EventArgs e)
        {
            SelectMaxPlus1_GO();
        }

        private void label80_MouseEnter(object sender, EventArgs e)
        {
            label80.BackColor = Color.Firebrick;
        }

        private void label80_MouseLeave(object sender, EventArgs e)
        {
            label80.BackColor = Color.FromArgb(58, 89, 114);
        }

        private void label81_MouseEnter(object sender, EventArgs e)
        {
            label81.BackColor = Color.Firebrick;
        }

        private void label81_MouseLeave(object sender, EventArgs e)
        {
            label81.BackColor = Color.FromArgb(58, 89, 114);
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseDown = true;
            lastLocation = e.Location;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            IsProcessOpen(); 
        }

        int i = 1;
        DateTime dt = new DateTime();
        private void timer2_Tick(object sender, EventArgs e)
        {
            label_stopwatch.Text = dt.AddSeconds(i).ToString("HH:mm:ss");
            i++;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Close();
            BackToMainMenu backtomainmenu = new BackToMainMenu();
            backtomainmenu.Show();
        }

        // Save *.sql file
        private void label83_Click(object sender, EventArgs e)
        {
            if (comboBox11.SelectedIndex == 2)
                comboBox11.SelectedIndex = 0;

            GenerateQuery();

            if (NUD_Entry.Text == "0")
            {
                MessageBox.Show("Entry should not be 0", "Error");
                return;
            }
            if (NUD_Entry.Text == "")
            {
                MessageBox.Show("Entry should not be empty", "Error");
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "sql files (*.sql)|*.sql";
                sfd.FilterIndex = 2;
                                            // [entry]       &       Name
                sfd.FileName = "GameObject[" + NUD_Entry.Value + "]" + textBox2.Text;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, stringSqlShare);

                    timer7.Start();
                }
            }
        }
        
        // COPY TO CLIPBOARD label
        private void label86_Click(object sender, EventArgs e)
        {
            if (comboBox11.SelectedIndex == 2)
                comboBox11.SelectedIndex = 0;

            GenerateQuery();

            if (NUD_Entry.Text == "0")
            {
                MessageBox.Show("Entry should not be 0", "Error");
                return;
            }
            if (NUD_Entry.Text == "")
            {
                MessageBox.Show("Entry should not be empty", "Error");
                return;
            }

            Clipboard.SetText(stringSqlShare);

            //label87.Visible = true;
            timer5.Start();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            label_query_executed_successfully2.Visible = true;
            label_Success.Visible = true;
            timer3.Stop();

            timer4.Start();
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            label_query_executed_successfully2.Visible = false;
            label_Success.Visible = false;
            timer4.Stop();
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            label_query_copied_to_clipboard.Visible = true;
            timer5.Stop();

            timer6.Start();

        }

        private void timer6_Tick(object sender, EventArgs e)
        {
            label_query_copied_to_clipboard.Visible = false;
            timer6.Stop();
        }

        private void timer7_Tick(object sender, EventArgs e)
        {
            label_saved_successfully.Visible = true;
            timer7.Stop();

            timer8.Start();
        }

        private void timer8_Tick(object sender, EventArgs e)
        {
            label_saved_successfully.Visible = false;
            timer8.Stop();
        }

        private void label81_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label80_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://emucraft.com");
        }

        private void label35_MouseHover(object sender, EventArgs e)
        {
            label35.ForeColor = Color.RoyalBlue;
        }

        private void label35_MouseLeave(object sender, EventArgs e)
        {
            label35.ForeColor = Color.Blue;
        }

        private void textBox28_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void label78_Click(object sender, EventArgs e)
        {
            Close();
            BackToMainMenu backtomainmenu = new BackToMainMenu(form_MM);
            backtomainmenu.Show();
        }

        private void label78_MouseEnter(object sender, EventArgs e)
        {
            label78.BackColor = Color.Firebrick;
        }

        private void label78_MouseLeave(object sender, EventArgs e)
        {
            label78.BackColor = Color.FromArgb(58, 89, 114);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label83_MouseEnter(object sender, EventArgs e)
        {
            panel5.BackColor = Color.Firebrick;
        }

        private void label83_MouseLeave(object sender, EventArgs e)
        {
            panel5.BackColor = Color.FromArgb(58, 89, 114);
        }
        //==========================================================
        private void label86_MouseEnter(object sender, EventArgs e)
        {
            panel7.BackColor = Color.Firebrick;
        }

        private void label86_MouseLeave(object sender, EventArgs e)
        {
            panel7.BackColor = Color.FromArgb(58, 89, 114);
        }

        private void panel5_MouseEnter(object sender, EventArgs e)
        {
            panel5.BackColor = Color.Firebrick;
        }

        private void panel5_MouseLeave(object sender, EventArgs e)
        {
            panel5.BackColor = Color.FromArgb(58, 89, 114);
        }

        private void panel7_MouseEnter(object sender, EventArgs e)
        {
            panel7.BackColor = Color.Firebrick;
        }

        private void panel7_MouseLeave(object sender, EventArgs e)
        {
            panel7.BackColor = Color.FromArgb(58, 89, 114);
        }

        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            button22.Visible = false;
            btn_DeleteQuery.Visible = false;
            if (comboBox11.SelectedIndex == 0)
            {
                textBox105.Text = "INSERT";
                button22.Visible = true;
            }
            else if (comboBox11.SelectedIndex == 1)
            {
                textBox105.Text = "REPLACE";
                button22.Visible = true;
            }
            else if (comboBox11.SelectedIndex == 2) // Delete Option
            {
                if (form_MM.CB_NoMySQL.Checked || label_mysql_status2.Text == "Connection Lost - MySQL is not running")
                {
                    btn_DeleteQuery.Enabled = false;
                }

                btn_DeleteQuery.Visible = true;
            }
        }

        private void label92_MouseEnter(object sender, EventArgs e)
        {
            // MessageBox.Show("A file labled GameObjects.sql will be created in the same directory as the SpawnCreator vX.X executable. \nSo you can save multiple data rows in a single .sql file.", "SpawnCreator", MessageBoxButtons.OK, MessageBoxIcon.Information);
            toolTip1.SetToolTip(label92, "A file labeled GameObjects.sql will be created in the same directory as the SpawnCreator vX.X executable. \nSo you can save multiple data rows in a single .sql file.");
            toolTip1.AutoPopDelay = 10000; // 10 seconds
        }

        private void button_SaveInTheSameFile_Click(object sender, EventArgs e)
        {
            if (comboBox11.SelectedIndex == 2)
                comboBox11.SelectedIndex = 0;

            GenerateQuery();

            if (NUD_Entry.Text == "0")
            {
                MessageBox.Show("Entry should not be 0", "Error");
                return;
            }
            if (NUD_Entry.Text == "")
            {
                MessageBox.Show("Entry should not be empty", "Error");
                return;
            }

            using (var writer = File.AppendText("GameObjects.sql"))
            {
                writer.Write(stringSqlShare + Environment.NewLine);
                button_SaveInTheSameFile.Text = "Saved!";
                button_SaveInTheSameFile.TextAlign = ContentAlignment.MiddleCenter;
            }
        }

        private void button_SaveInTheSameFile_MouseEnter(object sender, EventArgs e)
        {
            button_SaveInTheSameFile.BackColor = Color.Firebrick;
        }

        private void button_SaveInTheSameFile_MouseLeave(object sender, EventArgs e)
        {
            button_SaveInTheSameFile.BackColor = Color.FromArgb(58, 89, 114);
        }

        private void comboBox3_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(comboBox3, "This field is overridden by ScriptName field if both are set. \nOnly 'SmartGameObjectAI' can be used.");
            toolTip1.AutoPopDelay = 8000;
        }

        private void ALL_textBoxes_MouseEnter(object sender, EventArgs e)
        {
            button_SaveInTheSameFile.Text = "Save in the same file";
            button_SaveInTheSameFile.TextAlign = ContentAlignment.MiddleRight;
        }

        private void NUD_Entry_ValueChanged(object sender, EventArgs e)
        {
            ALL_textBoxes_MouseEnter(sender, e);
        }

        private void All_ComboBoxes_MouseEnter(object sender, EventArgs e)
        {
            ALL_textBoxes_MouseEnter(sender, e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            object sender = new object();
            EventArgs e = new EventArgs();

            if (keyData == Keys.F2)
            {
                button_SaveInTheSameFile_Click(sender, e); // Save in the same file if "F2" key is pressed
                return true;
            }

            else if (keyData == Keys.F5)
            {
                button_execute_query_Click(sender, e); // Execute Query if "F5" key is pressed
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btn_DeleteQuery_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to delete GameObject " + NUD_Entry.Text + " ?", "SpawnCreator " + form_MM.version, MessageBoxButtons.YesNo);
            if (dr == DialogResult.No)
                return;

            else
                DeleteGO();
        }

        void CopyAction(object sender, EventArgs e)
        {
            if (RTB_Notes.SelectedText.Length == 0)
                return;

            else
            Clipboard.SetText(RTB_Notes.SelectedText);
        }

        void PasteAction(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText(TextDataFormat.Rtf))
            {
                RTB_Notes.SelectedRtf
                    = Clipboard.GetData(DataFormats.Rtf).ToString();
            }
        }

        private void RTB_Notes_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu contextMenu = new ContextMenu();
                MenuItem menuItem = new MenuItem();
                menuItem = new MenuItem("Copy");
                menuItem.Click += new EventHandler(CopyAction);
                contextMenu.MenuItems.Add(menuItem);
                menuItem.Click += new EventHandler(PasteAction);
                contextMenu.MenuItems.Add(menuItem);

                RTB_Notes.ContextMenu = contextMenu;
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (comboBox11.SelectedIndex == 0)
                MessageBox.Show("MySQL - INSERT Syntax: \nInserts new rows into an existing table. ", "SpawnCreator", MessageBoxButtons.OK, MessageBoxIcon.Information);

            else
                MessageBox.Show("MySQL - REPLACE Syntax: \nREPLACE works exactly like INSERT, except that if an old row in the table has the same value as a new row for a PRIMARY KEY or a UNIQUE index, the old row is deleted before the new row is inserted.", "SpawnCreator", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
