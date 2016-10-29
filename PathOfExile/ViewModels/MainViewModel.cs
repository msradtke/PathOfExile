using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathOfExile.Models.Entities;
using PathOfExile.Models.Concrete;
using PathOfExile.Models.Services;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows;
using System.Threading;
using System.Media;
namespace PathOfExile.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private List<Player> _players;
        private PlayerRepository _playerRepository;
        private HistoryRepository _historyRepository;
        private ItemRepository _itemRepository;
        private Stopwatch _stopWatch;
        private WebScraper _webScraper;
        private List<Scrape> _scrapes;
        TaskScheduler scheduler;
        SynchronizationContext uiSync;
        CancellationTokenSource cancelSource;
        CancellationToken cancelToken;
        private readonly System.Object MyLock = new System.Object();
        public MainViewModel()
        {
            uiSync = SynchronizationContext.Current;
            LoadDataService.InstantiateFiles();
            _stopWatch = new Stopwatch();
            _playerRepository = new PlayerRepository();
            _historyRepository = new HistoryRepository();
            _itemRepository = new ItemRepository();
            HistoryEntry = _historyRepository.Get() as List<HistoryEntry>;
            Items = _itemRepository.Get() as List<Item>;
            cancelSource = new CancellationTokenSource();
            IsCopyStopped = true;
            IsCopyStarted = false;
            StartCopyCommand = new ActionCommand(StartCopy,()=>true);
            StopCopyCommand = new ActionCommand(StopCopy, () => true);
            NewItemCommand = new ActionCommand(AddNewItem, () => true);
            RemoveItemCommand = new ActionCommand(RemoveItem, () => true);
            SaveItemCommand = new ActionCommand(SaveItem, () => true);
            RemoveAttemptedCommand = new ActionCommand(RemoveAttempted, () => true);
            ScrapeCommand = new ActionCommand(Scrape, () => true);
            UseCurrentScrapedCommand = new ActionCommand(UseCurrentScraped, () => true);

            //_players = _playerRepository.Get().ToList();
            //_history = _historyRepository.Get().ToList();
        }

        public ICommand RemoveAttemptedCommand { get; private set; }
        public ICommand ScrapeCommand { get; private set; }
        public ICommand UseCurrentScrapedCommand { get; private set; }
        public ICommand StopCopyCommand { get; private set; }
        public ICommand StartCopyCommand { get; private set; }
        public ICommand NewItemCommand { get; private set; }
        public ICommand RemoveItemCommand { get; private set; }
        public ICommand SaveItemCommand { get; private set; }

        private List<Item> _items;
        public List<Item> Items
        {
            get { return _items; }
            set
            {
                if (value == _items)
                    return;

                _items = value;
                RaisePropertyChanged("Items");
            }
        }
        private List<HistoryEntry> _history;
        public List<HistoryEntry> HistoryEntry
        {
            get { return _history; }
            set
            {
                if (value == _history)
                    return;

                _history = value;
                RaisePropertyChanged("HistoryEntry");
            }
        }


        private Item _selectedItem;
        public Item SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value == _selectedItem)
                    return;

                _selectedItem = value;
                if (value != null)
                {
                    SelectedItemName = _selectedItem.ItemName;
                    SelectedItemLink = _selectedItem.Url;
                    MessageMulti = _selectedItem.MessageMulti;
                    MessageSingle = _selectedItem.MessageSingle;
                }
                RaisePropertyChanged("SelectedItem");
            }
        }

        private List<Link> _links;
        public List<Link> Links
        {
            get { return _links; }
            set
            {
                if (value == _links)
                    return;

                _links = value;
                RaisePropertyChanged("Links");
            }
        }

        private bool _isCopyStarted;
        public bool IsCopyStarted
        {
            get { return _isCopyStarted; }
            set
            {
                if (value == _isCopyStarted)
                    return;

                _isCopyStarted = value;
                RaisePropertyChanged("IsCopyStarted");
            }
        }

        private bool _isCopyStopped;
        public bool IsCopyStopped
        {
            get { return _isCopyStopped; }
            set
            {
                if (value == _isCopyStopped)
                    return;

                _isCopyStopped = value;
                RaisePropertyChanged("IsCopyStopped");
            }
        }

        private string _selectedItemLink;
        public string SelectedItemLink
        {
            get { return _selectedItemLink; }
            set
            {
                if (value == _selectedItemLink)
                    return;

                _selectedItemLink = value;
                RaisePropertyChanged("SelectedItemLink");
            }
        }

        private string _selectedItemName;
        public string SelectedItemName
        {
            get { return _selectedItemName; }
            set
            {
                if (value == _selectedItemName)
                    return;

                _selectedItemName = value;
                RaisePropertyChanged("SelectedItemName");
            }
        }

        private string _itemName;
        public string ItemName
        {
            get { return _itemName; }
            set
            {
                if (value == _itemName)
                    return;

                _itemName = value;
                RaisePropertyChanged("ItemName");
            }
        }

        private string _messageSingle;
        public string MessageSingle
        {
            get { return _messageSingle; }
            set
            {
                if (value == _messageSingle)
                    return;

                _messageSingle = value;
                RaisePropertyChanged("MessageSingle");
            }
        }

        private string _parsedMessageSingle;
        public string ParsedMessageSingle
        {
            get { return _parsedMessageSingle; }
            set
            {
                if (value == _parsedMessageSingle)
                    return;

                _parsedMessageSingle = value;
                RaisePropertyChanged("ParsedMessageSingle");
            }
        }

        private string _parsedMessageMulti;
        public string ParsedMessageMulti
        {
            get { return _parsedMessageMulti; }
            set
            {
                if (value == _parsedMessageMulti)
                    return;

                _parsedMessageMulti = value;
                RaisePropertyChanged("ParsedMessageMulti");
            }
        }

        private string _messageMulti;
        public string MessageMulti
        {
            get { return _messageMulti; }
            set
            {
                if (value == _messageMulti)
                    return;

                _messageMulti = value;
                RaisePropertyChanged("MessageMulti");
            }
        }

        private string _link;
        public string Link
        {
            get { return _link; }
            set
            {
                if (value == _link)
                    return;

                _link = value;
                RaisePropertyChanged("Link");
            }
        }

        private double _getInterval;
        public double GetInterval
        {
            get { return _getInterval; }
            set
            {
                if (value == _getInterval)
                    return;

                _getInterval = value;
                RaisePropertyChanged("GetInterval");
            }
        }

        private double _copyTimeElapsed;
        public double CopyTimeElapsed
        {
            get { return _copyTimeElapsed; }
            set
            {
                if (value == _copyTimeElapsed)
                    return;

                _copyTimeElapsed = value;
                RaisePropertyChanged("CopyTimeElapsed");
            }
        }

        private double _timeSinceLastGet;
        public double TimeSinceLastGet
        {
            get { return _timeSinceLastGet; }
            set
            {
                if (value == _timeSinceLastGet)
                    return;

                _timeSinceLastGet = value;
                RaisePropertyChanged("TimeSinceLastGet");
            }
        }

        private List<Scrape> _scrapedList;
        public List<Scrape> ScrapedList
        {
            get { return _scrapedList; }
            set
            {
                if (value == _scrapedList)
                    return;

                _scrapedList = value;
                RaisePropertyChanged("ScrapedList");
            }
        }

        private ObservableCollection<Contact> _playerContactList;
        public ObservableCollection<Contact> PlayerContactList
        {
            get { return _playerContactList; }
            set
            {
                if (value == _playerContactList)
                    return;
                
                _playerContactList = value;
                RaisePropertyChanged("PlayerContactList");
            }
        }

        private Contact _selectedPlayerContact;
        public Contact SelectedPlayerContact
        {
            get { return _selectedPlayerContact; }
            set
            {
                if (value == _selectedPlayerContact)
                    return;
                
                _selectedPlayerContact = value;
                RaisePropertyChanged("SelectedPlayerContact");
            }
        }

        private int _copyDelay;
        public int CopyDelay
        {
            get { return _copyDelay; }
            set
            {
                if (value == _copyDelay)
                    return;

                _copyDelay = value;
                RaisePropertyChanged("CopyDelay");
            }
        }

        private int _numberOfSets;
        public int NumberOfSets
        {
            get { return _numberOfSets; }
            set
            {
                if (value == _numberOfSets)
                    return;

                _numberOfSets = value;
                RaisePropertyChanged("NumberOfSets");
            }
        }

        private int _currentCopyCount;
        public int CurrentCopyCount
        {
            get { return _currentCopyCount; }
            set
            {
                if (value == _currentCopyCount)
                    return;

                _currentCopyCount = value;
                RaisePropertyChanged("CurrentCopyCount");
            }
        }

        private int _quantityPerSet;
        public int QuantityPerSet
        {
            get { return _quantityPerSet; }
            set
            {
                if (value == _quantityPerSet)
                    return;

                _quantityPerSet = value;
                RaisePropertyChanged("QuantityPerSet");
            }
        }

        private int _playerContactIndex;
        public int PlayerContactIndex
        {
            get { return _playerContactIndex; }
            set
            {
                
                if (value == _playerContactIndex)
                    return;
                    _playerContactIndex = value;
                    RaisePropertyChanged("PlayerContactIndex");
                
            }
        }

        #region Methods
        private void SelectedItemChanged()
        {
            ItemName = _selectedItemName;
            SelectedItemLink = Items.Where(x => x.ItemName == ItemName).FirstOrDefault().Url;
            ParseMessages();
            //retrieve link from storage

        }

        private void ParseMessages()
        {
            ParsedMessageSingle = MessageSingle.Replace("%name%", ItemName);
            ParsedMessageMulti = MessageMulti.Replace("%name%", ItemName);
        }

        private void SaveLinks()
        {
            var link = _links.Where(x => x.ItemName == _selectedItemName).FirstOrDefault();
            if (link != null)
                link.Url = _selectedItemLink;
        }

        private void RemoveAttempted()
        {
            ScrapedList = _scrapedList.Where(x => x.Attempted == false).ToList();
        }

        private void Scrape()
        {
            _webScraper = new WebScraper();
            ScrapedList = _webScraper.Start(SelectedItem);
            ModifyScrapedBasedOnHistory();
        }

        private void ModifyScrapedBasedOnHistory()
        {
           
            var contacted = HistoryEntry.Where(x => x.TimesContacted > 0 && x.Item == SelectedItemName);
            foreach(var scrape in ScrapedList)
            {
                var attempted = contacted.Any(x=>string.Equals(x.PlayerName,scrape.PlayerName));
                if (attempted)
                    scrape.Attempted = true;
            }

        }

        private void UseScrapes()
        {

        }

        private void StopCopy()
        {
            cancelSource.Cancel();
            IsCopyStopped = true;
            IsCopyStarted = false;
        }

        private void StartCopy()
        {

            if (PlayerContactIndex < 0)
                PlayerContactIndex = 0;
            IsCopyStopped = false;
            IsCopyStarted = true;
            CurrentCopyCount = 0;
            StartTimer();


            CopierLoop();
            //reportTask.Wait();
        }

        private void StartTimer()
        {
            _stopWatch.Restart();
        }

        private void CopierLoop()
        {
            cancelSource = new CancellationTokenSource();
            cancelToken = cancelSource.Token;
            
            Task reportTask = Task.Factory.StartNew(
() =>
{

            bool loop = true;
            if (IsCopyStarted)
            {
                while (loop)
                {
                    if (cancelToken.IsCancellationRequested)
                        return;
                    if (CurrentCopyCount == 0 ||
                        _stopWatch.ElapsedMilliseconds >= CurrentCopyCount * CopyDelay * 1000)
                    {
                        uiSync.Post((x) => CopyMessageToClipBoard(), null);
                        ++CurrentCopyCount;
                        
                        //Application.Current.Dispatcher.Invoke(new Action(() => { CopyMessageToClipBoard(); }),System.Windows.Threading.DispatcherPriority.DataBind);
                        
                        //Thread thread = new Thread(() => CopyMessageToClipBoard());
                        //thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                        //thread.Start();
                        //thread.Join();
                    }
                    if (CurrentCopyCount >= QuantityPerSet)
                        loop = false;
                }
                IsCopyStopped = true;
                IsCopyStarted = false;
            }
}
, cancelToken
);
        }


        private void CopyMessageToClipBoard()
        {
            if(PlayerContactList != null)
            {
                if (PlayerContactIndex < PlayerContactList.Count)
                {
                    var currentContact = PlayerContactList[PlayerContactIndex];


                    if (currentContact != null)
                    {
                        AddHistoryEntry();
                        SelectedPlayerContact.Attempted = true;
                        Clipboard.SetText(currentContact.Message);
                        (new SoundPlayer(@"C:\Users\Public\Documents\Unity Projects\Standard Assets Example Project\Assets\Standard Assets\Characters\FirstPersonCharacter\Audio\Jump.wav")).Play();
                        //System.Console.Beep();

                    }
                    ++PlayerContactIndex;
                }
            }
        }

        private void SaveHistory()
        {
            _historyRepository.Save();
        }

        private void AddHistoryEntry()
        {
            var history = new HistoryEntry();
            history.Item = SelectedPlayerContact.ItemName;
            history.PlayerName = SelectedPlayerContact.PlayerName;
            history.Quantity = SelectedPlayerContact.Quantity;
            history.TimesContacted++;
            history.TimeStamp = DateTime.Now;
            _history.Add(history);
            SaveHistory();
            HistoryEntry = _historyRepository.Get() as List<HistoryEntry>;
        }

        private void AddNewItem()
        {
            var item = new Item();
            Items.Add(item);
            SelectedItem = item;
        } 

        private void SaveItem()
        {
            SelectedItem.ItemName = SelectedItemName;
            SelectedItem.Url = SelectedItemLink;
            SelectedItem.MessageMulti = MessageMulti;
            SelectedItem.MessageSingle = MessageSingle;

            var Test = Items;
            Items = new List<Item>();
            Items = Test;
            _itemRepository.Save();
        }

        private void RemoveItem()
        {
            Items.Remove(SelectedItem);
            var Test = Items;
            Items = new List<Item>();
            Items = Test;
            SelectedItem = Items.FirstOrDefault();
        }

        private void UseCurrentScraped()
        {
            PlayerContactList = new ObservableCollection<Contact>();
            foreach(var scrape in ScrapedList)
            {
                var contact = new Contact();
                contact.Quantity = scrape.Quantity;
                if(contact.Quantity > 1)
                    contact.Message = "@" + scrape.PlayerName +" "+ SelectedItem.MessageMulti;
                else
                    contact.Message = "@" + scrape.PlayerName + " " + SelectedItem.MessageSingle;
                contact.PlayerName = scrape.PlayerName;
                contact.ItemName = scrape.Item;
                contact.Attempted = scrape.Attempted;
                PlayerContactList.Add(contact);
            }
        }

        #endregion
    }

}
