using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathOfExile.Models.Entities;
using PathOfExile.Models.Concrete;
namespace PathOfExile.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private List<Player> _players;
        private List<HistoryEntry> _history;
        private RepositoryBase<Player> _playerRepository;
        private RepositoryBase<HistoryEntry> _historyRepository;


        public MainViewModel()
        {
            _playerRepository = new RepositoryBase<Player>();
            _historyRepository = new RepositoryBase<HistoryEntry>();
            //_players = _playerRepository.Get().ToList();
            //_history = _historyRepository.Get().ToList();
        }


    }
}
