curl https://api.openai.com/v1/chat/completions -H "Authorization: Bearer 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"   -d '{  "model": "gpt-4",  "messages":     {      "role": "user",      "content": "Create a list of 8 questions for an interview with a science fiction author."    },          "role": "assistant",      "content": "1. What inspired you to write in the science fiction genre?\n2. How do you create the different worlds and technologies in your books?\n3. Can you share some of your favorite science fiction authors and how they have influenced your work?\n4. How do you balance scientific accuracy with storytelling in your works?\n5. What is the process you follow when developing characters for your science fiction novels?\n6. Can you share some challenges you face when writing science fiction and how you overcome them?\n7. How do you think the science fiction genre has evolved over the years and where do you see it going in the future?\n8. Can you give us a sneak peek into any upcoming projects or books you are currently working on?"    }  ],  "temperature": 0.5,  "max_tokens": 1024,  "top_p": 1,  "frequency_penalty": 0,  "presence_penalty": 0}'