using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.AudioFormat;

namespace YourCheese.GameAgent.Conversation
{
    class MeetingTalker
    {

        PlayerInformation botInfo;
        GameDataContainer gameData;

        public MeetingTalker(PlayerInformation botInfo, GameDataContainer gameData)
        {
            this.botInfo = botInfo;
            this.gameData = gameData;
        }

        public void tellTheMemory(RoundMemory roundMemory, PlayerInformation reportedBody, SkeldMap map)
        {
            String text = "";
            if (!reportedBody.position.IsGarbage())
            {
                text += "I found " + reportedBody.color + "'s body in " + map.getLocationRegionName(map.gamePosToMeshPos(reportedBody.position)) + ".";
            }
            foreach (var myEvent in roundMemory.witnessedEvents)
            {
                if (myEvent is DeathEvent)
                {
                    DeathEvent deathEvent = (DeathEvent)myEvent;
                    PlayerInformation killer;
                    if (!botInfo.isImposter)
                    {
                        killer = deathEvent.killer;
                        text += "I watched " + deathEvent.killer.color + " murder " + deathEvent.victim.color + " in " + map.getLocationRegionName(map.gamePosToMeshPos(deathEvent.position)) + ".";
                    }
                    else
                    {
                       
                    }
                    
                }
                else if (myEvent is VentEvent)
                {
                    if (!botInfo.isImposter)
                    {
                        VentEvent ventEvent = (VentEvent)myEvent;
                        text += ventEvent.venter.color + " vented right in front of me in " + map.getLocationRegionName(map.gamePosToMeshPos(ventEvent.position)) + ".";
                    }
                }
            }
            if (roundMemory.strategies.Count > 0)
                text += "This round, I was ";
            foreach (var strat in roundMemory.strategies)
            {
                text += strat.getAsString() + ", ";
            }
            if (roundMemory.getTrustedPlayers().Count > 0)
            text += ". I trust ";
            foreach (var player in roundMemory.getTrustedPlayers())
            {
                text += player.color + ", ";
            }
            SpeakTheText(text);
            roundMemory.refresh();
        }

        public void SpeakTheText(string text)
        {
            // Initialize a new instance of the speech synthesizer.  
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            using (MemoryStream streamAudio = new MemoryStream())
            {
                // Create a SoundPlayer instance to play the output audio file.  
                System.Media.SoundPlayer m_SoundPlayer = new System.Media.SoundPlayer();
                // Set voice to male
                synth.SelectVoiceByHints(VoiceGender.Female);
                // Configure the synthesizer to output to an audio stream.  
                synth.SetOutputToWaveStream(streamAudio);

                // Speak a phrase.  
                synth.Speak(text);
                streamAudio.Position = 0;
                m_SoundPlayer.Stream = streamAudio;
                m_SoundPlayer.Play();

                // Set the synthesizer output to null to release the stream.   
                synth.SetOutputToNull();

                // Insert code to persist or process the stream contents here.  
            }
        }

        public void VoiceIntoFile(String text, String file)
        {
            // Initialize a new instance of the SpeechSynthesizer.  
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                synth.SelectVoiceByHints(VoiceGender.Female);
                // Configure the audio output.   
                synth.SetOutputToWaveFile($@"C:\Studio\{file}",
                  new SpeechAudioFormatInfo(32000, AudioBitsPerSample.Sixteen, AudioChannel.Mono));

                // Create a SoundPlayer instance to play output audio file.  
                System.Media.SoundPlayer m_SoundPlayer =
                  new System.Media.SoundPlayer($@"C:\Studio\{file}");

                // Build a prompt.  
                PromptBuilder builder = new PromptBuilder();
                builder.AppendText(text);

                // Speak the prompt.  
                synth.Speak(builder);
                m_SoundPlayer.Play();
            }
        }
    
    }
}
