using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab7
{
    public partial class Form1 : Form
    {
        private IChatRoom chatRoom;
        private Dictionary<string, Participant> participants = new Dictionary<string, Participant>();

        public Form1()
        {
            InitializeComponent();
            //initializing chat room
            chatRoom = new ChatRoom();

            //creating participants
            participants["User1"] = new User("User1");
            participants["User2"] = new User("User2");

            //registrating participants with the chat room
            foreach (var participant in participants.Values)
            {
                chatRoom.Register(participant);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string message = txtMessage.Text.Trim();
            if (!string.IsNullOrEmpty(message))
            {
                //send message to chat room
                participants["User1"].Send(message); //User1 is the sender for this example

                //display sent message in chat history
                lstChatHistory.Items.Add($"User1: {message}");

                //clear message textbox
                txtMessage.Clear();
            }
        }

        private void btnSent2_Click(object sender, EventArgs e)
        {
            string message = txtMessage2.Text.Trim();
            if (!string.IsNullOrEmpty(message))
            {
                //send message to chat room
                participants["User2"].Send(message); //User2 is the sender for this example

                //display sent message in chat history
                lstChatHistory.Items.Add($"User2: {message}");

                //clear message textbox
                txtMessage.Clear();
            }
        }


    }






    public abstract class Participant
    {
        public string Name { get; }
        public IChatRoom ChatRoom { get; set; } 

        public Participant(string name)
        {
            Name = name;
        }

        public void Send(string message)
        {
            ChatRoom.SendMessage(message, this);
        }

        public abstract void ReceiveMessage(string message);
    }


    public class User : Participant
    {
        public User(string name) : base(name) { }

        public override void ReceiveMessage(string message) { }
    }

    //mediator interface
    public interface IChatRoom
    {
        void SendMessage(string message, Participant participant);
        void Register(Participant participant);
    }


    //concrete mediator
    public class ChatRoom : IChatRoom
    {
        private Dictionary<string, Participant> participants = new Dictionary<string, Participant>();

        public void Register(Participant participant)
        {
            participants[participant.Name] = participant;
            participant.ChatRoom = this;
        }
        public void SendMessage(string message, Participant sender)
        {
            foreach (var participant in participants.Values)
            {
                if (participant != sender) //exclude the sender
                    participant.ReceiveMessage(message);
            }
        }

    }

}

