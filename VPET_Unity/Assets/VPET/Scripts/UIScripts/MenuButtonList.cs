﻿/*
-----------------------------------------------------------------------------
This source file is part of VPET - Virtual Production Editing Tool
http://vpet.research.animationsinstitut.de/
http://github.com/FilmakademieRnd/VPET

Copyright (c) 2016 Filmakademie Baden-Wuerttemberg, Institute of Animation

This project has been realized in the scope of the EU funded project Dreamspace
under grant agreement no 610005.
http://dreamspaceproject.eu/

This program is free software; you can redistribute it and/or modify it under
the terms of the GNU Lesser General Public License as published by the Free Software
Foundation; version 2.1 of the License.

This program is distributed in the hope that it will be useful, but WITHOUT
ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License along with
this program; if not, write to the Free Software Foundation, Inc., 59 Temple
Place - Suite 330, Boston, MA 02111-1307, USA, or go to
http://www.gnu.org/licenses/old-licenses/lgpl-2.1.html
-----------------------------------------------------------------------------
*/
﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace vpet
{
	//!
	//! List button inherit from Button and implement IMenuButton interface. On click the action is called and the button switch to the next state,
	//! i.e. change button icon and set to next action.
	//!
	public class MenuButtonList : Button, IMenuButton
	{
		//!
		//! Reference to the menu this button is included 
		//!
		private Menu menu;
		public Menu Menu
		{
			set{ menu = value; }
		}

		//!
		//! Toggle state. ( this type of button is not toggleable )
		//!
		public bool Toggled
		{
			get { return false; }
            set { }
		}

		private List<Sprite> spritesActive = new List<Sprite>();
		private List<Sprite> spritesIdle = new List<Sprite>();
		private List<UnityAction> actions = new List<UnityAction>();

		private int spriteIdx = 0;

		//!
		//! Adds an action to this button. This button type can hold several actions which will be cycled through.
		//! @param      active      sprite displayed when clicked/toggled
		//! @param      idle      	sprite displayed in normal state
		//! @param      call      	method to be called onclick
		//!
		public void AddAction(Sprite active, Sprite idle, UnityAction call)
		{
			if ( spritesActive.Count == 0 ) // first
			{
				SpriteState newSprites = new SpriteState();
				newSprites.disabledSprite = idle;
				newSprites.highlightedSprite = idle;
				newSprites.pressedSprite = active;
				spriteState = newSprites;
				this.GetComponent<Image>().sprite = idle;
			}

			spritesActive.Add(active);
			spritesIdle.Add(idle);
			actions.Add(call);
		}


		//!
		//! Override method. Called when button is clicked.
		//!
		public override void OnPointerClick (PointerEventData eventData)
		{
			// set active button at menu
			menu.ActiveButton = this.gameObject;

			// count index
			spriteIdx = (spriteIdx+1) % spritesActive.Count;

			// change  sprite state
			SpriteState spriteStateTmp = spriteState;
			spriteStateTmp.highlightedSprite = spritesIdle[spriteIdx];
			spriteStateTmp.pressedSprite = spritesActive[spriteIdx];
			spriteState = spriteStateTmp;
			this.GetComponent<Image>().sprite = spritesIdle[spriteIdx];

			// call
			actions[spriteIdx]();

			// call base
			base.OnPointerClick (eventData);
		}

        public void AddHoldAction(UnityAction call)
        {
            // empty
        }

        //!
        //! Reset button state
        //!
        public void reset()	{}

	}
}