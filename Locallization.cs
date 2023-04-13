using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using BepInEx;
using UnityEngine;

namespace EpicMMOSystem;

public class Localization
{
    private string defaultFileName = "eng_emmosLocalization.txt";
    private Dictionary<string, string> _dictionary = new ();

    public Localization()
    {
        var currentLanguage = global::Localization.instance.GetSelectedLanguage();
        if (currentLanguage == "Russian")
        {
            RusLocalization();
        }
        else if (currentLanguage == "English")
        {
            EngLocalization();
        }
        else if (currentLanguage == "Spanish")
        {
            SpanLocalization();
        }
        else if (currentLanguage == "German")
        {
            GermanLocalization();
        }
        else if (currentLanguage == "Chinese")
        {
            ChineseLocalization();
        }
        else
        {
            var fileName = $"{EpicMMOSystem.language.Value}_emmosLocalization.txt";
            var basePath = Path.Combine(Paths.PluginPath, EpicMMOSystem.ModName, fileName);
            if (File.Exists(basePath))
            {
                ReadLocalization(basePath);
                return;
            }
            CreateLocalizationFile();
        }
    }

    private void ReadLocalization(string path)
    {
        var lines = File.ReadAllLines(path);
        EngLocalization();
        bool update = _dictionary.Count > lines.Length;
        foreach (var line in lines)
        {
            var pair = line.Split('=');
            var text = pair[1].Replace('*', '\n');
            _dictionary[pair[0].Trim()] = text.TrimStart();
        }
        if (update)
        {
            List<string> list = new List<string>();
            foreach (var pair in _dictionary)
            {
                list.Add($"{pair.Key} = {pair.Value}");
            }
            File.WriteAllLines(path, list);
        }
    }

    private void CreateLocalizationFile()
    {
        EngLocalization();
        List<string> list = new List<string>();
        foreach (var pair in _dictionary)
        {
            list.Add($"{pair.Key} = {pair.Value}");
        }
        DirectoryInfo dir = new DirectoryInfo(Paths.PluginPath);
        dir.CreateSubdirectory(Path.Combine(Paths.PluginPath, EpicMMOSystem.ModName));
        File.WriteAllLines(Path.Combine(Paths.PluginPath, EpicMMOSystem.ModName, defaultFileName), list);
    }

    private void RusLocalization()
    {
        _dictionary.Add("$attributes", "Параметры");
        _dictionary.Add("$parameter_strength", "Сила");
        _dictionary.Add("$parameter_agility", "Ловкость");
        _dictionary.Add("$parameter_intellect", "Интеллект");
        _dictionary.Add("$parameter_body", "Телосложение");
        _dictionary.Add("$free_points", "Доступно очков");
        _dictionary.Add("$level", "Уровень");
        _dictionary.Add("$lvl", "Ур.");
        _dictionary.Add("$exp", "Опыт");
        _dictionary.Add("$cancel", "Отмена");
        _dictionary.Add("$apply", "Принять");
        _dictionary.Add("$reset_parameters", "Сбросить параметры");
        _dictionary.Add("$no", "Нет");
        _dictionary.Add("$yes", "Да");
        _dictionary.Add("$get_exp", "Получено опыта");
        _dictionary.Add("$reset_point_text", "Вы действительно хотите сбросить все поинты за {0} {1}?");
        //Parameter
        _dictionary.Add("$physic_damage", "Ув. физ. урона");
        _dictionary.Add("$add_weight", "Ув. переносимого веса");
        _dictionary.Add("$speed_attack", "Расход вын. на атаку");
        _dictionary.Add("$reduced_stamina", "Расход вын. (бег, прыжок)");
        _dictionary.Add("$magic_damage", "Ув. маг. урона");
        _dictionary.Add("$magic_armor", "Ув. маг. защиты");
        _dictionary.Add("$add_hp", "Ув. здоровья");
        _dictionary.Add("$add_stamina", "Ув. выносливости");
        _dictionary.Add("$physic_armor", "Ув. физ. защиты");
        _dictionary.Add("$reduced_stamina_block", "Расход вын. на блок");
        _dictionary.Add("$regen_hp", "Регенерация здоровья");
        _dictionary.Add("$damage", "Урон");
        _dictionary.Add("$armor", "Защита");
        _dictionary.Add("$survival", "Выживание");

        _dictionary.Add("$regen_eitr", "регенерация Eitr");
        _dictionary.Add("$stamina_reg", "Регенерация выносливости"); 
        _dictionary.Add("$add_eitr", "Повышение Eitr");
        //Friends list
        _dictionary.Add("$notify", "<color=#00E6FF>Оповещение</color>");
        _dictionary.Add("$friends_list", "Список друзей");
        _dictionary.Add("$send", "Отправить");
        _dictionary.Add("$invited", "Приглашения");
        _dictionary.Add("$friends", "Друзья");
        _dictionary.Add("$online", "В игре");
        _dictionary.Add("$offline", "Нет в игре");
        _dictionary.Add("$not_found", "Игрок {0} не найден.");
        _dictionary.Add("$send_invite", "Игроку {0}, отправлен запрос в друзья.");
        _dictionary.Add("$get_invite", "Получен запрос в друзья от {0}.");
        _dictionary.Add("$accept_invite", "Игрок {0}, принял запрос в друзья.");
        _dictionary.Add("$cancel_invite", "Игрок {0}, отменил запрос в друзья.");
        //Terminal
        _dictionary.Add("$terminal_set_level", "Вы получили {0} уровень");
        _dictionary.Add("$terminal_reset_points", "Ваши очки характеристик были сброшены");
    }
    private void EngLocalization()
    {
        _dictionary.Add("$attributes", "Attributes");
        _dictionary.Add("$parameter_strength", "Strength");
        _dictionary.Add("$parameter_agility", "Agility");
        _dictionary.Add("$parameter_intellect", "Intellect");
        _dictionary.Add("$parameter_body", "Endurance");
        _dictionary.Add("$free_points", "Available points");
        _dictionary.Add("$level", "Level");
        _dictionary.Add("$lvl", "Lvl.");
        _dictionary.Add("$exp", "Experience");
        _dictionary.Add("$cancel", "Cancel");
        _dictionary.Add("$apply", "Accept");
        _dictionary.Add("$reset_parameters", "Reset points");
        _dictionary.Add("$no", "No");
        _dictionary.Add("$yes", "Yes");
        _dictionary.Add("$get_exp", "Experience received");
        _dictionary.Add("$reset_point_text", "Do you really want to drop all the points for {0} {1}?");
        //Parameter
        _dictionary.Add("$physic_damage", "Physical Damage");
        _dictionary.Add("$add_weight", "Carry weight");
        _dictionary.Add("$speed_attack", "Attack stamina consumption");
        _dictionary.Add("$reduced_stamina", "Stamina consumption (running, jumping)");
        _dictionary.Add("$magic_damage", "Elemental damage");
        _dictionary.Add("$magic_armor", "Elemental reduced");
        _dictionary.Add("$add_hp", "Health increase");
        _dictionary.Add("$add_stamina", "Stamina increase");
        _dictionary.Add("$physic_armor", "Physical reduced");
        _dictionary.Add("$reduced_stamina_block", "Block stamina consumption");
        _dictionary.Add("$regen_hp", "Health regeneration");
        _dictionary.Add("$damage", "Damage");
        _dictionary.Add("$armor", "Armor");
        _dictionary.Add("$survival", "Survival");

        _dictionary.Add("$regen_eitr", "Eitr regeneration");
        _dictionary.Add("$stamina_reg", "Stamina regeneration");
        _dictionary.Add("$add_eitr", "Eitr Increase");
        //Friends list
        _dictionary.Add("$notify", "<color=#00E6FF>Alert</color>");
        _dictionary.Add("$friends_list", "Friends list");
        _dictionary.Add("$send", "Send");
        _dictionary.Add("$invited", "Invitations");
        _dictionary.Add("$friends", "Friends");
        _dictionary.Add("$online", "Online");
        _dictionary.Add("$offline", "Offline");
        _dictionary.Add("$not_found", "Player {0} is not found.");
        _dictionary.Add("$send_invite", "A friend request has been sent to player {0}.");
        _dictionary.Add("$get_invite", "Received a friend request from {0}.");
        _dictionary.Add("$accept_invite", "Player {0}, accepted the friend request.");
        _dictionary.Add("$cancel_invite", "Player {0}, canceled his friend request.");
        //Terminal
        _dictionary.Add("$terminal_set_level", "You got {0} level");
        _dictionary.Add("$terminal_reset_points", "Your attributes points have been reset");
    }

    private void SpanLocalization()
    {
        _dictionary.Add("$attributes", "Atributos");
        _dictionary.Add("$parameter_strength", "Fuerza");
        _dictionary.Add("$parameter_agility", "Agilidad");
        _dictionary.Add("$parameter_intellect", "Intelecto");
        _dictionary.Add("$parameter_body", "Resistencia");
        _dictionary.Add("$free_points", "Puntos disponibles");
        _dictionary.Add("$level", "Nivel");
        _dictionary.Add("$lvl", "Nivel.");
        _dictionary.Add("$exp", "Experiencia");
        _dictionary.Add("$cancel", "Cancelar");
        _dictionary.Add("$apply", "Aceptar");
        _dictionary.Add("$reset_parameters", "Restablecer puntos");
        _dictionary.Add("$no", "No");
        _dictionary.Add("$yes", "Si");
        _dictionary.Add("$get_exp", "Experiencia recibida");
        _dictionary.Add("$reset_point_text", "¿De verdad quieres eliminar todos los puntos por {0} {1}?");
        //Parameter
        _dictionary.Add("$physic_damage", "Daño Físico");
        _dictionary.Add("$add_weight", "Cargar Peso");
        _dictionary.Add("$speed_attack", "Consumo de resistencia de ataque");
        _dictionary.Add("$reduced_stamina", "Consumo de energía (correr, saltar)");
        _dictionary.Add("$magic_damage", "Daño mágico");
        _dictionary.Add("$magic_armor", "Armadura mágica");
        _dictionary.Add("$add_hp", "Aumento de la salud");
        _dictionary.Add("$add_stamina", "Aumento de resistencia");
        _dictionary.Add("$physic_armor", "Armadura física");
        _dictionary.Add("$reduced_stamina_block", "Consumo de energia de bloqueo");
        _dictionary.Add("$regen_hp", "Regeneración de salud");
        _dictionary.Add("$damage", "Daño");
        _dictionary.Add("$armor", "Armadura");
        _dictionary.Add("$survival", "Surpervivencia");

        _dictionary.Add("$regen_eitr", "regeneración de eitr");
        _dictionary.Add("$stamina_reg", "regeneración de resistencia");
        _dictionary.Add("$add_eitr", "Aumento de EIT");
        //Friends list
        _dictionary.Add("$notify", "<color=#00E6FF>Alerta</color>");
        _dictionary.Add("$friends_list", "Lista de amigos");
        _dictionary.Add("$send", "Enviar");
        _dictionary.Add("$invited", "Invitaciones");
        _dictionary.Add("$friends", "Amigos");
        _dictionary.Add("$online", "Conectado");
        _dictionary.Add("$offline", "Desconectado");
        _dictionary.Add("$not_found", "Player {0} inexistente.");
        _dictionary.Add("$send_invite", "Se ha enviado una solicitud de amistad a {0}.");
        _dictionary.Add("$get_invite", "Has recivido una solicitud de amistad de {0}.");
        _dictionary.Add("$accept_invite", "{0} aceptó la solicitud de amistad.");
        _dictionary.Add("$cancel_invite", "{0} denegó la solicitud de amistad.");
        //Terminal
        _dictionary.Add("$terminal_set_level", "Eres nivel {0}");
        _dictionary.Add("$terminal_reset_points", "Tus puntos de atributos han sido reiniciados");
    }

    private void GermanLocalization()
    {

        _dictionary.Add("$attributes", "Eigenschaften");
        _dictionary.Add("$parameter_strength", "Stärke");
        _dictionary.Add("$parameter_agility", "Beweglichkeit");
        _dictionary.Add("$parameter_intellect", "Intelligenz");
        _dictionary.Add("$parameter_body", "Ausdauer");
        _dictionary.Add("$free_points", "Verfügbare Punkte");
        _dictionary.Add("$level", "Level");
        _dictionary.Add("$lvl", "Lvl.");
        _dictionary.Add("$exp", "Erfahrung");
        _dictionary.Add("$cancel", "Zurück");
        _dictionary.Add("$apply", "übernehmen");
        _dictionary.Add("$reset_parameters", "Punkte zurücksetzen");
        _dictionary.Add("$no", "Nein");
        _dictionary.Add("$yes", "Ja");
        _dictionary.Add("$get_exp", "Erfahrung erhalten");
        _dictionary.Add("$reset_point_text", "Möchtest du wirklich alle Punkte Löschen? {0} {1}?");

        _dictionary.Add("$physic_damage", "Körperlicher Schaden");
        _dictionary.Add("$add_weight", "Gewicht tragen");
        _dictionary.Add("$speed_attack", "Ausdauerverbrauch angreifen");
        _dictionary.Add("$reduced_stamina", "Ausdauerverbrauch (running, jumping)");
        _dictionary.Add("$magic_damage", "Elementarschaden");
        _dictionary.Add("$magic_armor", " Elementarschaden Rüstung");
        _dictionary.Add("$add_hp", "Gesundheitssteigerung");
        _dictionary.Add("$add_stamina", "Ausdauer erhöt um");
        _dictionary.Add("$physic_armor", "Physische Rüstung");
        _dictionary.Add("$reduced_stamina_block", "Ausdauerverbrauch blockieren");
        _dictionary.Add("$regen_hp", "Heilung");
        _dictionary.Add("$damage", "Schaden");
        _dictionary.Add("$armor", "Rüstung");
        _dictionary.Add("$survival", "überleben");

        _dictionary.Add("$regen_eitr", "Eitr-Regeneration");
        _dictionary.Add("$stamina_reg", "Regeneration der Ausdauer");
        _dictionary.Add("$add_eitr", "Eitr erhöhen");

        _dictionary.Add("$notify", "<color=#00E6FF>Alarm");
        _dictionary.Add("$friends_list", "Freundesliste");
        _dictionary.Add("$send", "Senden");
        _dictionary.Add("$invited", "Einladungen");
        _dictionary.Add("$friends", "Freunde");
        _dictionary.Add("$online", "Online");
        _dictionary.Add("$offline", "Offline");
        _dictionary.Add("$not_found", "Spieler {0} nicht gefunden.");
        _dictionary.Add("$send_invite", "Eine Freundschaftsanfrage wurde an den Spieler gesendet {0}.");
        _dictionary.Add("$get_invite", "erhielt eine Freundschaftsanfrage von {0}.");
        _dictionary.Add("$accept_invite", "Spieler {0}, hat die Freundschaftsanfrage bestätigt.");
        _dictionary.Add("$cancel_invite", "Spieler {0}, hat die Freundschaftsanfrage abgelehnt.");
        _dictionary.Add("$terminal_set_level", "Du hast {0} level");
        _dictionary.Add("$terminal_reset_points", "Deine Attributspunkte wurden zurückgesetzt");

    }

    private void ChineseLocalization()
    {
        _dictionary.Add("$attributes", "角色属性");
        _dictionary.Add("$parameter_strength", "力量");
        _dictionary.Add("$parameter_agility", "敏捷");
        _dictionary.Add("$parameter_intellect", "智力");
        _dictionary.Add("$parameter_body", "体质");
        _dictionary.Add("$free_points", "可分配的属性点");
        _dictionary.Add("$level", "等级");
        _dictionary.Add("$lvl", "等级：");
        _dictionary.Add("$exp", "经验");
        _dictionary.Add("$cancel", "取消");
        _dictionary.Add("$apply", "接受");
        _dictionary.Add("$reset_parameters", "重置属性");
        _dictionary.Add("$no", "否");
        _dictionary.Add("$yes", "是");
        _dictionary.Add("$get_exp", "所需经验");
        _dictionary.Add("$reset_point_text", "你是否要消耗 {0} {1} 重置你的所有属性吗? ");
        _dictionary.Add("$physic_damage", "物理伤害");
        _dictionary.Add("$add_weight", "负重");
        _dictionary.Add("$speed_attack", "攻击耐力值消耗");
        _dictionary.Add("$reduced_stamina", "耐力值消耗（跑步、跳跃）");
        _dictionary.Add("$magic_damage", "元素伤害");
        _dictionary.Add("$magic_armor", "元素抗性");
        _dictionary.Add("$add_hp", "生命值");
        _dictionary.Add("$add_stamina", "耐力值");
        _dictionary.Add("$physic_armor", "物理抗性");
        _dictionary.Add("$reduced_stamina_block", "格挡耐力值消耗");
        _dictionary.Add("$regen_hp", "生命值恢复");
        _dictionary.Add("$damage", "攻击属性");
        _dictionary.Add("$armor", "防御属性");
        _dictionary.Add("$survival", "基础属性");
        _dictionary.Add("$regen_eitr", "埃达之力恢复");
        _dictionary.Add("$stamina_reg", "耐力值恢复");
        _dictionary.Add("$add_eitr", "埃达之力");
        _dictionary.Add("$notify", "<color=#00E6FF>通知</color>");
        _dictionary.Add("$friends_list", "好友列表");
        _dictionary.Add("$send", "发送");
        _dictionary.Add("$invited", "邀请");
        _dictionary.Add("$friends", "好友");
        _dictionary.Add("$online", "在线");
        _dictionary.Add("$offline", "离线");
        _dictionary.Add("$not_found", "未找到 玩家 {0} ！");
        _dictionary.Add("$send_invite", "已向 玩家 {0} 发送好友请求。");
        _dictionary.Add("$get_invite", "收到来自 玩家 {0} 的好友请求。");
        _dictionary.Add("$accept_invite", "玩家 {0} , 已接受好友请求。");
        _dictionary.Add("$cancel_invite", "玩家 {0} , 拒绝了好友请求。");
        _dictionary.Add("$terminal_set_level", "您提升到了 {0} 级！");
        _dictionary.Add("$terminal_reset_points", "您的属性已重置！");
    }

    public string this[string key]
    {
        get
        {
            if (_dictionary.ContainsKey(key))
            {
                return _dictionary[key];
            }
            return "Missing language key";
        }
    }
}