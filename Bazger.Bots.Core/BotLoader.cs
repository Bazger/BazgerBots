using System;
using System.Collections.Generic;
using System.IO;
using Bazger.Bots.Core.Utils;
using log4net;

namespace Bazger.Bots.Core
{
    public class BotLoader
    {
        private readonly BotStateBuilder _botState;
        private ILog _logger = LogManager.GetLogger("AllLoggers");

        private readonly List<BotComponent> _components;
        private readonly string _botStateBackupDir;
        private readonly bool _loadFromBackup;
        private readonly string _botStateFileName;

        public BotLoader(List<BotComponent> components, Type driverType = null, string botStateBackupDir = null,
            bool loadFromBackup = false)
        {
            _components = components;
            _loadFromBackup = loadFromBackup;
            _botStateBackupDir = botStateBackupDir;
            _botStateFileName = "BotStateBuilder.state";
            _botState = new BotStateBuilder(driverType);
            if (!loadFromBackup == false)
            {
                if (!String.IsNullOrEmpty(_botStateBackupDir))
                {
                    _botState =
                        SerDeUtils.DeserializeJsonFile<BotStateBuilder>(Path.Combine(_botStateBackupDir, _botStateFileName));
                    _logger.Info($"BotState loaded from {_botStateFileName}");
                }
                else
                {
                    _logger.Warn($"Can't find state file on path:{_botStateBackupDir}");
                }
            }
        }

        public void Start()
        {
            if (!_loadFromBackup)
            {
                StartFromNewBotState();
            }
            else
            {
                StartFromOldBotState();
            }
        }

        private void StartFromNewBotState()
        {
            try
            {
                //Prepare stage
                _botState.RunningStage = ComponentsStages.Prepare;
                foreach (var botComponent in _components)
                {
                    if (botComponent.ToRun)
                    {
                        _botState.RunningComponent = botComponent.GetType().Name;
                        botComponent.Prepare(_botState);
                        _logger.Debug($"PREPARE stage for {botComponent.GetType().Name} succeed");
                    }
                }

                //Process stage
                _botState.RunningStage = ComponentsStages.Process;
                foreach (var botComponent in _components)
                {
                    if (botComponent.ToRun)
                    {
                        _botState.RunningComponent = botComponent.GetType().Name;
                        botComponent.Process(_botState);
                        _logger.Debug($"PROCESS stage for {botComponent.GetType().Name} succeed");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                SaveBotState();
                throw ex;
            }
        }

        private void StartFromOldBotState()
        {
            //TODO: Lauch from backup
        }

        public void SaveBotState()
        {
            if (!String.IsNullOrEmpty(_botStateBackupDir))
            {
                var savePath = Path.Combine(_botStateBackupDir, _botStateFileName);
                SerDeUtils.SerializeToJsonFile(_botState, savePath);
                _logger.Info($"BotState successfully saved: {savePath}");
            }
            else
            {
                _logger.Warn($"Can't save BotState, bad path: {_botStateBackupDir}");
            }
        }

    }
}